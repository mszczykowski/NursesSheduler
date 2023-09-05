using NursesScheduler.BusinessLogic.Abstractions.Infrastructure.Providers;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.Constants;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.Services
{
    public sealed class QuarterStatsService : IQuarterStatsService
    {
        private readonly ICalendarService _calendarService;
        private readonly IDepartamentSettingsProvider _departamentSettingsProvider;
        private readonly IScheduleStatsProvider _scheduleStatsProvider;

        public QuarterStatsService(ICalendarService calendarService, IScheduleStatsProvider scheduleStatsProvider,
            IDepartamentSettingsProvider departamentSettingsProvider)
        {
            _calendarService = calendarService;
            _departamentSettingsProvider = departamentSettingsProvider;
            _scheduleStatsProvider = scheduleStatsProvider;
        }

        public async Task InvalidateQuarterCacheAsync(int year, int quarterNumber, int departamentId)
        {
            var departamentSettings = await _departamentSettingsProvider.GetCachedDataAsync(departamentId);

            var keys = await GetStatsKeysQuarterSchedulesAsync(year, quarterNumber, departamentSettings);

            foreach(var key in keys)
            {
                _scheduleStatsProvider.InvalidateCache(key);
            }
        }

        public async Task<NurseStats> RecalculateQuarterNurseStatsAsync(NurseStats currentScheduleNursesStats,
            int year, int month, int departamentId)
        {
            var remainingQuarterSchedulesStats = await GetRemainingQuarterSchedulesStatsAsync(year, month, departamentId);

            var quarterSchedulesNurseStats = new List<NurseStats>
            {
                currentScheduleNursesStats,
            };
            quarterSchedulesNurseStats.AddRange(remainingQuarterSchedulesStats
                .Select(s => s.NursesScheduleStats.FirstOrDefault(s => s.NurseId == currentScheduleNursesStats.NurseId))
                .OfType<NurseStats>());

            return GetQuarterNurseStats(quarterSchedulesNurseStats);
        }

        public async Task<QuarterStats> GetQuarterStatsAsync(ScheduleStats currentScheduleStats, int year, int month, 
            int departamentId)
        {
            var quarterSchedulesStats = new List<ScheduleStats>
            {
                currentScheduleStats,
            };
            quarterSchedulesStats.AddRange(await GetRemainingQuarterSchedulesStatsAsync(year, month, departamentId));

            var workTimeInQuarter = CalculateWorkTimeInQuarter(quarterSchedulesStats);

            var quarterStats = new QuarterStats
            {
                WorkTimeInQuarter = workTimeInQuarter,
                TimeForMorningShifts = CalculateTimeForMorningShifts(workTimeInQuarter),
                ShiftsToAssignInMonths = CalculateShiftsToAssignInMonths(quarterSchedulesStats, workTimeInQuarter),
            };

            quarterStats.NurseStats = GetQuarterNursesStats(quarterSchedulesStats, quarterStats.ShiftsToAssignInMonths);

            return quarterStats;
        }

        private int[] CalculateShiftsToAssignInMonths(IEnumerable<ScheduleStats> quarterScheduleStats, 
            TimeSpan workTimeInQuarter)
        {
            var totalNumberOfShifts = (int)Math.Floor(workTimeInQuarter / ScheduleConstatns.RegularShiftLength);

            var shiftToAssignInMonths = new int[3];

            for (var i = 0; i < shiftToAssignInMonths.Length; i++)
            {
                var currentScheduleStats = quarterScheduleStats.First(s => s.MonthInQuarter == i + 1);
                shiftToAssignInMonths[i] = (int)Math.Round(currentScheduleStats.WorkTimeInMonth 
                    / ScheduleConstatns.RegularShiftLength);
            }

            while (shiftToAssignInMonths.Sum() < totalNumberOfShifts)
            {
                shiftToAssignInMonths[Array.IndexOf(shiftToAssignInMonths, shiftToAssignInMonths.Min())]++;
            }
            while (shiftToAssignInMonths.Sum() > totalNumberOfShifts)
            {
                shiftToAssignInMonths[Array.IndexOf(shiftToAssignInMonths, shiftToAssignInMonths.Max())]--;
            }

            return shiftToAssignInMonths;
        }

        private async Task<IEnumerable<ScheduleStatsKey>> GetStatsKeysQuarterSchedulesAsync(int year, int quarterNumber,
            DepartamentSettings departamentSettings)
        {
            var quarterMonths = _calendarService.GetQuarterMonths(year, quarterNumber,
                departamentSettings.FirstQuarterStart);

            var keys = new List<ScheduleStatsKey>();

            foreach (var monthYear in quarterMonths)
            {
                keys.Add(new ScheduleStatsKey
                {
                    Year = monthYear.Year,
                    Month = monthYear.Month,
                    DepartamentId = departamentSettings.DepartamentId,
                });
            }

            return keys;
        }

        private async Task<IEnumerable<ScheduleStatsKey>> GetStatsKeysQuarterSchedulesAsync(int year, int month,
            int departamentId)
        {
            var departamentSettings = await _departamentSettingsProvider.GetCachedDataAsync(departamentId);
            
            var quarterNumber = _calendarService.GetQuarterNumber(month, departamentSettings.FirstQuarterStart);

            return await GetStatsKeysQuarterSchedulesAsync(year, quarterNumber, departamentSettings);
        }

        private IEnumerable<NurseQuarterStats> GetQuarterNursesStats(IEnumerable<ScheduleStats> quarterSchedulesStats,
            int[] shiftToAssignInMonths)
        {
            var nursesQuarterStats = new List<NurseQuarterStats>();

            var nursesIds = quarterSchedulesStats
                .SelectMany(s => s.NursesScheduleStats)
                .Select(n => n.NurseId).Distinct();

            foreach (var nurseId in nursesIds)
            {
                var nurseQuarterStats = GetQuarterNurseStats(quarterSchedulesStats
                    .SelectMany(q => q.NursesScheduleStats)
                    .Where(s => s.NurseId == nurseId));

                nurseQuarterStats.HadNumberOfShiftsReduced 
                    = GetHadNumberOfShiftsReduced(shiftToAssignInMonths, quarterSchedulesStats, nurseId);

                nursesQuarterStats.Add(nurseQuarterStats);
            }

            return nursesQuarterStats;
        }

        private async Task<ICollection<ScheduleStats>> GetRemainingQuarterSchedulesStatsAsync(int currentYear,
            int currentMonth, int departamentId)
        {
            var schedulesKeys = await GetStatsKeysQuarterSchedulesAsync(currentYear, currentMonth, departamentId);

            var quarterScheduleStats = new List<ScheduleStats>();

            foreach (var key in schedulesKeys.Where(k => k.Month != currentMonth))
            {
                quarterScheduleStats.Add(await _scheduleStatsProvider.GetCachedDataAsync(key));
            }

            return quarterScheduleStats;
        }

        private NurseQuarterStats GetQuarterNurseStats(IEnumerable<NurseStats> quarterSchedulesNurseStats)
        {
            var nurseQuarterStats = new NurseQuarterStats
            {
                NurseId = quarterSchedulesNurseStats.First().NurseId,
                AssignedMorningShiftsIds = new List<int>(),
                WorkTimeAssignedInWeeks = new Dictionary<int, TimeSpan>(),
            };

            foreach (var scheduleNurseStats in quarterSchedulesNurseStats)
            {
                nurseQuarterStats.AssignedWorkTime += scheduleNurseStats.AssignedWorkTime;
                nurseQuarterStats.HolidayHoursAssigned += scheduleNurseStats.HolidayHoursAssigned;
                nurseQuarterStats.NightHoursAssigned += scheduleNurseStats.NightHoursAssigned;
                nurseQuarterStats.TimeOffToAssign += scheduleNurseStats.TimeOffToAssign;
                nurseQuarterStats.TimeOffAssigned += scheduleNurseStats.TimeOffAssigned;

                if(scheduleNurseStats.AssignedMorningShiftsIds is not null)
                {
                    nurseQuarterStats.AssignedMorningShiftsIds = nurseQuarterStats.AssignedMorningShiftsIds
                        .Union(scheduleNurseStats.AssignedMorningShiftsIds);
                }

                if(scheduleNurseStats.WorkTimeAssignedInWeeks is not null)
                {
                    foreach (var week in scheduleNurseStats.WorkTimeAssignedInWeeks)
                    {
                        if (!nurseQuarterStats.WorkTimeAssignedInWeeks.ContainsKey(week.Key))
                        {
                            nurseQuarterStats.WorkTimeAssignedInWeeks.Add(week.Key, week.Value);
                        }
                        else
                        {
                            nurseQuarterStats.WorkTimeAssignedInWeeks[week.Key] += week.Value;
                        }
                    }
                }
            }

            return nurseQuarterStats;
        }

        private TimeSpan CalculateWorkTimeInQuarter(IEnumerable<ScheduleStats> quarterScheduleStats)
        {
            return TimeSpan.FromTicks(quarterScheduleStats.Sum(q => q.WorkTimeInMonth.Ticks));
        }

        private TimeSpan CalculateTimeForMorningShifts(TimeSpan workTimeInQuarter)
        {
            return workTimeInQuarter - ((int)Math.Floor(workTimeInQuarter / ScheduleConstatns.RegularShiftLength)
                * ScheduleConstatns.RegularShiftLength);
        }

        private bool GetHadNumberOfShiftsReduced(int[] shiftsToAssignInMonths, 
            IEnumerable<ScheduleStats> quarterScheduleStats, int nurseId)
        {
            foreach(var scheduleStats in quarterScheduleStats)
            {
                var nurseStats = scheduleStats.NursesScheduleStats.FirstOrDefault(s => s.NurseId == nurseId);
                
                if(nurseStats is null)
                {
                    continue;
                }

                if((int)Math.Floor(nurseStats.AssignedWorkTime / ScheduleConstatns.RegularShiftLength)
                    == shiftsToAssignInMonths[scheduleStats.MonthInQuarter - 1] - 1)
                {
                    return true;
                }
            }

            return false;
        }
    }
}

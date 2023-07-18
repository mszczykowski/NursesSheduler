using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure.Providers;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain;
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
            };

            quarterStats.NurseStats = GetQuarterNursesStats(quarterSchedulesStats);

            return quarterStats;
        }

        private async Task<IEnumerable<ScheduleStatsKey>> GetStatsKeysQuarterSchedulesAsync(int year, int month,
            int departamentId)
        {
            var departamentSettings = await _departamentSettingsProvider.GetCachedDataAsync(departamentId);

            var quarterNumber = _calendarService.GetQuarterNumber(month, departamentSettings.FirstQuarterStart);

            var quarterMonths = _calendarService.GetQuarterMonths(year, quarterNumber,
                departamentSettings.FirstQuarterStart);

            var keys = new List<ScheduleStatsKey>();

            foreach (var monthYear in quarterMonths)
            {
                keys.Add(new ScheduleStatsKey
                {
                    Year = monthYear.Year,
                    Month = monthYear.Month,
                    DepartamentId = departamentId,
                });
            }

            return keys;
        }

        private IEnumerable<NurseStats> GetQuarterNursesStats(IEnumerable<ScheduleStats> quarterSchedulesStats)
        {
            var nursesQuarterStats = new List<NurseStats>();

            var nursesIds = quarterSchedulesStats
                .SelectMany(s => s.NursesScheduleStats)
                .Select(n => n.NurseId).Distinct();

            foreach (var nurseId in nursesIds)
            {
                nursesQuarterStats.Add(GetQuarterNurseStats(quarterSchedulesStats
                    .SelectMany(q => q.NursesScheduleStats)
                    .Where(s => s.NurseId == nurseId)));
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

        private NurseStats GetQuarterNurseStats(IEnumerable<NurseStats> quarterSchedulesNurseStats)
        {
            var nurseQuarterStats = new NurseStats
            {
                NurseId = quarterSchedulesNurseStats.First().NurseId,
            };

            foreach (var scheduleNurseStats in quarterSchedulesNurseStats)
            {
                nurseQuarterStats.AssignedWorkTime += scheduleNurseStats.AssignedWorkTime;
                nurseQuarterStats.HolidayHoursAssigned += scheduleNurseStats.HolidayHoursAssigned;
                nurseQuarterStats.NightShiftsAssigned += scheduleNurseStats.NightShiftsAssigned;
                nurseQuarterStats.TimeOffToAssign += scheduleNurseStats.TimeOffToAssign;
                nurseQuarterStats.TimeOffAssigned += scheduleNurseStats.TimeOffAssigned;

                if(scheduleNurseStats.MorningShiftsIdsAssigned is not null)
                {
                    nurseQuarterStats.MorningShiftsIdsAssigned = nurseQuarterStats.MorningShiftsIdsAssigned
                        .Union(scheduleNurseStats.MorningShiftsIdsAssigned);
                }

                if(scheduleNurseStats.WorkTimeInWeeks is not null)
                {
                    foreach (var week in scheduleNurseStats.WorkTimeInWeeks)
                    {
                        if (!nurseQuarterStats.WorkTimeInWeeks.ContainsKey(week.Key))
                        {
                            nurseQuarterStats.WorkTimeInWeeks.Add(week.Key, week.Value);
                        }
                        else
                        {
                            nurseQuarterStats.WorkTimeInWeeks[week.Key] += week.Value;
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
            return workTimeInQuarter - ((int)Math.Floor(workTimeInQuarter / GeneralConstants.RegularShiftLenght)
                * GeneralConstants.RegularShiftLenght);
        }
    }
}

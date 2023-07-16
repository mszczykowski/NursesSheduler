using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure.Providers;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Enums;
using NursesScheduler.Domain.ValueObjects;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.Services
{
    internal sealed class StatsService : IStatsService
    {
        private readonly IApplicationDbContext _context;
        private readonly ICalendarService _calendarService;
        private readonly IDepartamentSettingsProvider _departamentSettingsProvider;
        private readonly IWorkTimeService _workTimeService;
        private readonly IScheduleStatsProvider _scheduleStatsProvider;

        public StatsService(IApplicationDbContext context, ICalendarService calendarService,
            IDepartamentSettingsProvider departamentSettingsProvider, IWorkTimeService workTimeService,
            IScheduleStatsProvider scheduleStatsProvider)
        {
            _context = context;
            _calendarService = calendarService;
            _departamentSettingsProvider = departamentSettingsProvider;
            _workTimeService = workTimeService;
            _scheduleStatsProvider = scheduleStatsProvider;
        }

        public async Task<IEnumerable<NurseStats>> GetQuarterNurseStats(ScheduleStats currentScheduleStats, int year,
            int month, int departamentId)
        {
            var quarterSchedulesStats = await GetAllQuarterStatsAsync(currentScheduleStats, year, month, departamentId);

            return GetQuarterNurseStats(quarterSchedulesStats);
        }

        public async Task<QuarterStats> GetQuarterStats(ScheduleStats currentScheduleStats, int year, int month,
            int departamentId)
        {
            var quarterSchedulesStats = await GetAllQuarterStatsAsync(currentScheduleStats, year, month, departamentId);

            var workTimeInQuarter = CalculateWorkTimeInQuarter(quarterSchedulesStats);

            var quarterStats = new QuarterStats
            {
                WorkTimeInQuarter = workTimeInQuarter,
                TimeForMorningShifts = CalculateTimeForMorningShifts(workTimeInQuarter),
            };

            quarterStats.NurseStats = GetQuarterNurseStats(quarterSchedulesStats);

            return quarterStats;
        }

        public async Task<ScheduleStats> GetScheduleStatsAsync(int year, int month, int departamentId)
        {
            var schedule = await _context.Schedules
                .Include(s => s.ScheduleNurses)
                .ThenInclude(s => s.NurseWorkDays)
                .ThenInclude(d => d.MorningShift)
                .Include(s => s.Quarter)
                .FirstOrDefaultAsync(s => s.Quarter.Year == year && s.Month == month
                    && s.Quarter.DepartamentId == departamentId);

            var departamentSettings = await _departamentSettingsProvider.GetCachedDataAsync(departamentId);

            var days = await _calendarService.GetMonthDaysAsync(year, month, departamentSettings.FirstQuarterStart);

            var scheduleStatsKey = new ScheduleStatsKey
            {
                Year = year,
                Month = month,
                DepartamentId = departamentId,
            };

            if (schedule is not null && schedule.IsClosed)
            {
                return GetStatsFromClosedSchedule(scheduleStatsKey, schedule, days, departamentSettings);
            }

            return GetScheduleStats(scheduleStatsKey, days, departamentSettings, schedule);
        }

        public async Task<ScheduleStats> GetScheduleStatsAsync(Schedule schedule, int departamentId, int year)
        {
            var departamentSettings = await _departamentSettingsProvider.GetCachedDataAsync(departamentId);

            var days = await _calendarService.GetMonthDaysAsync(year, schedule.Month,
                departamentSettings.FirstQuarterStart);

            var scheduleStatsKey = new ScheduleStatsKey
            {
                Year = year,
                Month = schedule.Month,
                DepartamentId = departamentId,
            };

            return GetScheduleStats(scheduleStatsKey, days, departamentSettings, schedule);
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

        public async Task<IEnumerable<NurseScheduleStats>> GetNurseScheduleStats(Schedule schedule, int departamentId,
            int year)
        {
            var departamentSettings = await _departamentSettingsProvider.GetCachedDataAsync(departamentId);
            var days = await _calendarService
                .GetMonthDaysAsync(year, schedule.Month, departamentSettings.FirstQuarterStart);

            return GetNurseScheduleStats(schedule, days, departamentSettings);
        }

        private IEnumerable<NurseStats> GetQuarterNurseStats(IEnumerable<ScheduleStats> quarterScheduleStats)
        {
            var result = new List<NurseStats>();

            foreach (var scheduleStats in quarterScheduleStats)
            {
                foreach (var nurseScheduleStats in scheduleStats.NursesScheduleStats)
                {
                    var nurseQuarterStats = result
                        .FirstOrDefault(s => s.NurseId == nurseScheduleStats.NurseId);

                    if (nurseQuarterStats is null)
                    {
                        nurseQuarterStats = new NurseStats
                        {
                            NurseId = nurseScheduleStats.NurseId,
                            AssignedWorkTime = nurseScheduleStats.AssignedWorkTime,
                            HolidayHoursAssigned = nurseScheduleStats.HolidayHoursAssigned,
                            NightShiftsAssigned = nurseScheduleStats.NightShiftsAssigned,
                            TimeOffToAssign = nurseScheduleStats.TimeOffToAssign,
                            TimeOffAssigned = nurseScheduleStats.TimeOffAssigned,
                            MorningShiftsAssigned = new List<MorningShiftIndex>(nurseScheduleStats.MorningShiftsAssigned),
                            WorkTimeInWeeks = new Dictionary<int, TimeSpan>(),
                        };
                    }
                    else
                    {
                        nurseQuarterStats.AssignedWorkTime += nurseScheduleStats.AssignedWorkTime;
                        nurseQuarterStats.HolidayHoursAssigned += nurseScheduleStats.HolidayHoursAssigned;
                        nurseQuarterStats.NightShiftsAssigned += nurseScheduleStats.NightShiftsAssigned;
                        nurseQuarterStats.TimeOffToAssign += nurseQuarterStats.TimeOffToAssign;
                        nurseQuarterStats.TimeOffAssigned += nurseQuarterStats.TimeOffAssigned;
                        nurseQuarterStats.MorningShiftsAssigned.Union(nurseQuarterStats.MorningShiftsAssigned);
                    }

                    foreach (var week in nurseScheduleStats.WorkTimeInWeeks)
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

            return result;
        }

        private ScheduleStats GetScheduleStats(ScheduleStatsKey key, IEnumerable<DayNumbered> days,
            DepartamentSettings departamentSettings, Schedule? schedule)
        {
            var workTimeInMonth = _workTimeService.GetWorkTimeFromDays(days, departamentSettings.WorkDayLength);

            var scheduleStats = new ScheduleStats
            {
                CacheKey = key,
                WorkTimeInMonth = workTimeInMonth,
                MonthInQuarter = _calendarService
                    .GetMonthInQuarterNumber(key.Month, departamentSettings.FirstQuarterStart),
                WorkTimeBalance = CalculateWorkTimeBalance(workTimeInMonth, schedule?.ScheduleNurses?.Count() ?? 0,
                    departamentSettings.TargetMinNumberOfNursesOnShift, days.Count())
            };

            scheduleStats.NursesScheduleStats = GetNurseScheduleStats(schedule, days, departamentSettings);

            return scheduleStats;
        }

        private IEnumerable<NurseScheduleStats> GetNurseScheduleStats(Schedule? schedule, IEnumerable<DayNumbered> days,
            DepartamentSettings departamentSettings)
        {
            var nurseScheduleStats = new List<NurseScheduleStats>();

            if (schedule is null)
            {
                return nurseScheduleStats;
            }

            foreach (var nurseSchedule in schedule.ScheduleNurses)
            {
                var nurseStats = new NurseScheduleStats
                {
                    AssignedWorkTime = CalculateAssignedWorkTime(nurseSchedule.NurseWorkDays),
                    HolidayHoursAssigned = CalculateHolidayHoursAssigned(nurseSchedule.NurseWorkDays, days,
                        departamentSettings),
                    NightShiftsAssigned = CalculateNightSiftsAssigned(nurseSchedule.NurseWorkDays),
                    LastState = GetLastState(nurseSchedule.NurseWorkDays),
                    HoursFromLastShift = GetHoursFromLastShift(nurseSchedule.NurseWorkDays),
                    MorningShiftsAssigned = GetAssignedMorningShifts(nurseSchedule.NurseWorkDays),
                    WorkTimeInWeeks = GetWorkTimeInWeeks(nurseSchedule.NurseWorkDays, days),
                    TimeOffToAssign = CalculateTimeOffToAssign(nurseSchedule.NurseWorkDays, days,
                        departamentSettings.WorkDayLength),
                    TimeOffAssigned = CalculateTimeOffAssigned(nurseSchedule.NurseWorkDays),
                };

                nurseScheduleStats.Add(nurseStats);
            }

            return nurseScheduleStats;
        }

        private int CalculateNightSiftsAssigned(IEnumerable<NurseWorkDay> nurseWorkDays)
        {
            return nurseWorkDays.Count(d => d.ShiftType == ShiftTypes.Night);
        }

        private TimeSpan CalculateHolidayHoursAssigned(IEnumerable<NurseWorkDay> nurseWorkDays, IEnumerable<Day> days,
            DepartamentSettings departamentSettings)
        {
            var holiadayHoursAssinged = TimeSpan.Zero;
            foreach (var day in days.Where(d => d.IsHoliday))
            {
                var workDay = nurseWorkDays.First(d => d.Day == day.Date.DayNumber);

                if (workDay.ShiftType == ShiftTypes.None)
                {
                    continue;
                }
                else if (workDay.ShiftType == ShiftTypes.Night)
                {
                    holiadayHoursAssinged += departamentSettings.NightShiftHolidayEligibleHours;
                }
                else if (workDay.ShiftType == ShiftTypes.Day)
                {
                    holiadayHoursAssinged += departamentSettings.DayShiftHolidayEligibleHours;
                }
                else if (workDay.ShiftType == ShiftTypes.Morning)
                {
                    holiadayHoursAssinged += workDay.MorningShift.ShiftLength;
                }
            }
            return holiadayHoursAssinged;
        }

        private TimeSpan CalculateAssignedWorkTime(IEnumerable<NurseWorkDay> nurseWorkDays)
        {
            return TimeSpan.FromTicks(nurseWorkDays.Sum(d => GetAssignedDayWorkTime(d).Ticks));
        }

        private ScheduleStats GetStatsFromClosedSchedule(ScheduleStatsKey key, Schedule schedule,
            IEnumerable<DayNumbered> days, DepartamentSettings departamentSettings)
        {
            var scheduleStats = new ScheduleStats
            {
                CacheKey = key,
                WorkTimeInMonth = schedule.WorkTimeInMonth,
                WorkTimeBalance = schedule.WorkTimeBalance,
                MonthInQuarter = _calendarService
                    .GetMonthInQuarterNumber(key.Month, departamentSettings.FirstQuarterStart),
            };

            var nurseScheduleStats = new List<NurseScheduleStats>();

            foreach (var nurseSchedule in schedule.ScheduleNurses)
            {
                var nurseStats = new NurseScheduleStats
                {
                    AssignedWorkTime = nurseSchedule.AssignedWorkTime,
                    HolidayHoursAssigned = nurseSchedule.HolidaysHoursAssigned,
                    NightShiftsAssigned = nurseSchedule.NightShiftsAssigned,
                    TimeOffToAssign = nurseSchedule.TimeOffToAssign,
                    TimeOffAssigned = nurseSchedule.TimeOffToAssiged,
                    LastState = GetLastState(nurseSchedule.NurseWorkDays),
                    HoursFromLastShift = GetHoursFromLastShift(nurseSchedule.NurseWorkDays),
                    MorningShiftsAssigned = GetAssignedMorningShifts(nurseSchedule.NurseWorkDays),
                    WorkTimeInWeeks = GetWorkTimeInWeeks(nurseSchedule.NurseWorkDays, days),
                };

                nurseScheduleStats.Add(nurseStats);
            }

            scheduleStats.NursesScheduleStats = nurseScheduleStats;

            return scheduleStats;
        }

        private IEnumerable<MorningShiftIndex> GetAssignedMorningShifts(IEnumerable<NurseWorkDay> nurseWorkDays)
        {
            return new List<MorningShiftIndex>(nurseWorkDays
                .Where(d => d.ShiftType == ShiftTypes.Morning)
                .Select(d => d.MorningShift.Index));
        }

        private ShiftTypes GetLastState(IEnumerable<NurseWorkDay> nurseWorkDays)
        {
            return nurseWorkDays.First(d => d.Day == nurseWorkDays.Max(d => d.Day)).ShiftType;
        }
        private TimeSpan GetHoursFromLastShift(IEnumerable<NurseWorkDay> nurseWorkDays)
        {
            TimeSpan hoursFromLastShift = TimeSpan.Zero;
            foreach (var workDay in nurseWorkDays.OrderByDescending(d => d.Day))
            {
                switch(workDay.ShiftType)
                {
                    case ShiftTypes.None:
                        hoursFromLastShift += TimeSpan.FromDays(1);
                        break;
                    case ShiftTypes.Day:
                        hoursFromLastShift += GeneralConstants.RegularShiftLenght;
                        return hoursFromLastShift;
                    case ShiftTypes.Night:
                        return hoursFromLastShift;
                    case ShiftTypes.Morning:
                        hoursFromLastShift += 2 * GeneralConstants.RegularShiftLenght - workDay.MorningShift.ShiftLength;
                        return hoursFromLastShift;
                }
            }
            return hoursFromLastShift;
        }

        private IDictionary<int, TimeSpan> GetWorkTimeInWeeks(IEnumerable<NurseWorkDay> nurseWorkDays,
            IEnumerable<DayNumbered> days)
        {
            var workTimeInWeeks = new Dictionary<int, TimeSpan>();

            foreach (var day in days)
            {
                if (!workTimeInWeeks.ContainsKey(day.WeekInQuarter))
                {
                    workTimeInWeeks.Add(day.WeekInQuarter, TimeSpan.Zero);
                }

                var workDay = nurseWorkDays.First(d => d.Day == day.Date.DayNumber);

                if (workDay.ShiftType == ShiftTypes.None)
                {
                    continue;
                }

                if (workDay.ShiftType == ShiftTypes.Morning)
                {
                    workTimeInWeeks[day.WeekInQuarter] += workDay.MorningShift.ShiftLength;
                }

                workTimeInWeeks[day.WeekInQuarter] += GeneralConstants.RegularShiftLenght;
            }

            return workTimeInWeeks;
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

        private TimeSpan CalculateTimeOffToAssign(IEnumerable<NurseWorkDay> nurseWorkDays,
            IEnumerable<Day> days, TimeSpan workDayLength)
        {
            return nurseWorkDays.Where(wd => wd.IsTimeOff && days.First(d => d.Date.Day == wd.Day).IsWorkingDay)
                .Count() * workDayLength;
        }

        private TimeSpan CalculateTimeOffAssigned(IEnumerable<NurseWorkDay> nurseWorkDays)
        {
            return TimeSpan
                .FromTicks(nurseWorkDays.Where(d => d.IsTimeOff == true).Sum(d => GetAssignedDayWorkTime(d).Ticks));
        }

        private TimeSpan GetAssignedDayWorkTime(NurseWorkDay nurseWorkDay)
        {
            switch (nurseWorkDay.ShiftType)
            {
                case ShiftTypes.None:
                    return TimeSpan.Zero;
                case ShiftTypes.Morning:
                    return nurseWorkDay.MorningShift.ShiftLength;
                default:
                    return GeneralConstants.RegularShiftLenght;
            }
        }

        private TimeSpan CalculateWorkTimeBalance(TimeSpan workTimeInMonth, int numberOfNurses,
            int minNumberOfNursesOnShift, int numberOfDaysInMonth)
        {
            return (workTimeInMonth * numberOfNurses)
                -
                (numberOfDaysInMonth * minNumberOfNursesOnShift * GeneralConstants.RegularShiftLenght);
        }

        private async Task<IEnumerable<ScheduleStats>> GetAllQuarterStatsAsync(ScheduleStats currentScheduleStats,
            int currentYear, int currentMonth, int departamentId)
        {
            var schedulesKeys = await GetStatsKeysQuarterSchedulesAsync(currentYear, currentMonth, departamentId);

            var quarterScheduleStats = new List<ScheduleStats>
            {
                currentScheduleStats,
            };

            foreach (var key in schedulesKeys.Where(k => k.Month != currentMonth))
            {
                quarterScheduleStats.Add(await _scheduleStatsProvider.GetCachedDataAsync(key));
            }

            return quarterScheduleStats;
        }
    }
}

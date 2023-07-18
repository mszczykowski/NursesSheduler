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
    internal sealed class ScheuleStatsService : IScheduleStatsService
    {
        private readonly IApplicationDbContext _context;
        private readonly ICalendarService _calendarService;
        private readonly IDepartamentSettingsProvider _departamentSettingsProvider;
        private readonly IWorkTimeService _workTimeService;

        public ScheuleStatsService(IApplicationDbContext context, ICalendarService calendarService,
            IDepartamentSettingsProvider departamentSettingsProvider, IWorkTimeService workTimeService)
        {
            _context = context;
            _calendarService = calendarService;
            _departamentSettingsProvider = departamentSettingsProvider;
            _workTimeService = workTimeService;
        }

        public async Task<NurseScheduleStats> RecalculateNurseScheduleStats(ScheduleNurse scheduleNurse,
            int departamentId, int year, int month)
        {
            var departamentSettings = await _departamentSettingsProvider.GetCachedDataAsync(departamentId);
            var days = await _calendarService
                .GetNumberedMonthDaysAsync(year, month, departamentSettings.FirstQuarterStart);

            return GetNurseScheduleStats(scheduleNurse, days, departamentSettings);
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

            var days = await _calendarService.GetNumberedMonthDaysAsync(year, month, departamentSettings.FirstQuarterStart);

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

            var days = await _calendarService.GetNumberedMonthDaysAsync(year, schedule.Month,
                departamentSettings.FirstQuarterStart);

            var scheduleStatsKey = new ScheduleStatsKey
            {
                Year = year,
                Month = schedule.Month,
                DepartamentId = departamentId,
            };

            return GetScheduleStats(scheduleStatsKey, days, departamentSettings, schedule);
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

            scheduleStats.NursesScheduleStats = GetNursesScheduleStats(schedule, days, departamentSettings);

            return scheduleStats;
        }

        private IEnumerable<NurseScheduleStats> GetNursesScheduleStats(Schedule? schedule, IEnumerable<DayNumbered> days,
            DepartamentSettings departamentSettings)
        {
            var nurseScheduleStats = new List<NurseScheduleStats>();

            if (schedule is null)
            {
                return nurseScheduleStats;
            }

            foreach (var scheduleNurse in schedule.ScheduleNurses)
            {
                nurseScheduleStats.Add(GetNurseScheduleStats(scheduleNurse, days, departamentSettings));
            }

            return nurseScheduleStats;
        }

        private NurseScheduleStats GetNurseScheduleStats(ScheduleNurse scheduleNurse, IEnumerable<DayNumbered> days,
            DepartamentSettings departamentSettings)
        {
            var nurseStats = new NurseScheduleStats
            {
                AssignedWorkTime = CalculateAssignedWorkTime(scheduleNurse.NurseWorkDays),
                HolidayHoursAssigned = CalculateHolidayHoursAssigned(scheduleNurse.NurseWorkDays, days,
                        departamentSettings),
                NightShiftsAssigned = CalculateNightSiftsAssigned(scheduleNurse.NurseWorkDays),
                LastState = GetLastState(scheduleNurse.NurseWorkDays),
                HoursFromLastShift = GetHoursFromLastShift(scheduleNurse.NurseWorkDays),
                MorningShiftsIdsAssigned = GetAssignedMorningShifts(scheduleNurse.NurseWorkDays),
                WorkTimeInWeeks = GetWorkTimeInWeeks(scheduleNurse.NurseWorkDays, days),
                TimeOffToAssign = CalculateTimeOffToAssign(scheduleNurse.NurseWorkDays, days,
                        departamentSettings.WorkDayLength),
                TimeOffAssigned = CalculateTimeOffAssigned(scheduleNurse.NurseWorkDays),
            };

            return nurseStats;
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
                    holiadayHoursAssinged += workDay.MorningShift.ShiftLength >
                        departamentSettings.DayShiftHolidayEligibleHours ?
                        departamentSettings.DayShiftHolidayEligibleHours :
                        workDay.MorningShift.ShiftLength;
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
                    MorningShiftsIdsAssigned = GetAssignedMorningShifts(nurseSchedule.NurseWorkDays),
                    WorkTimeInWeeks = GetWorkTimeInWeeks(nurseSchedule.NurseWorkDays, days),
                };

                nurseScheduleStats.Add(nurseStats);
            }

            scheduleStats.NursesScheduleStats = nurseScheduleStats;

            return scheduleStats;
        }

        private IEnumerable<int> GetAssignedMorningShifts(IEnumerable<NurseWorkDay> nurseWorkDays)
        {
            return new List<int>(nurseWorkDays
                .Where(d => d.ShiftType == ShiftTypes.Morning)
                .Select(d => d.MorningShift.MorningShiftId));
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
                switch (workDay.ShiftType)
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
    }
}

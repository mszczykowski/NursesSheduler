using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure.Providers;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Enums;
using NursesScheduler.Domain.ValueObjects;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.Services
{
    internal sealed class ScheduleStatsService : IScheduleStatsService
    {
        private readonly IApplicationDbContext _context;
        private readonly ICalendarService _calendarService;
        private readonly IDepartamentSettingsProvider _departamentSettingsProvider;
        private readonly IWorkTimeService _workTimeService;

        public ScheduleStatsService(IApplicationDbContext context, ICalendarService calendarService,
            IDepartamentSettingsProvider departamentSettingsProvider, IWorkTimeService workTimeService)
        {
            _context = context;
            _calendarService = calendarService;
            _departamentSettingsProvider = departamentSettingsProvider;
            _workTimeService = workTimeService;
        }

        public async Task<NurseScheduleStats> RecalculateNurseScheduleStats(int year, int month, int departamentId,
            ScheduleNurse scheduleNurse)
        {
            var departamentSettings = await _departamentSettingsProvider.GetCachedDataAsync(departamentId);
            var days = await _calendarService
                .GetNumberedMonthDaysAsync(year, month, departamentSettings.FirstQuarterStart);

            return BuildNurseScheduleStats(scheduleNurse, days, departamentSettings);
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

            var scheduleStatsKey = new ScheduleStatsKey
            {
                Year = year,
                Month = month,
                DepartamentId = departamentId,
            };

            return await GetScheduleStatsAsync(scheduleStatsKey, schedule);
        }

        public async Task<ScheduleStats> GetScheduleStatsAsync(int year, int departamentId, Schedule schedule)
        {
            var scheduleStatsKey = new ScheduleStatsKey
            {
                Year = year,
                Month = schedule.Month,
                DepartamentId = departamentId,
            };

            return await GetScheduleStatsAsync(scheduleStatsKey, schedule);
        }

        private async Task<ScheduleStats> GetScheduleStatsAsync(ScheduleStatsKey scheduleStatsKey, Schedule? schedule)
        {
            var departamentSettings = await _departamentSettingsProvider
                .GetCachedDataAsync(scheduleStatsKey.DepartamentId);

            var monthDays = await _calendarService
                .GetNumberedMonthDaysAsync(scheduleStatsKey.Year, scheduleStatsKey.Month,
                    departamentSettings.FirstQuarterStart);

            if (schedule is not null && schedule.IsClosed)
            {
                return GetStatsFromClosedSchedule(scheduleStatsKey, schedule, monthDays, departamentSettings);
            }

            return BuildScheduleStats(scheduleStatsKey, monthDays, departamentSettings, schedule);
        }

        private ScheduleStats BuildScheduleStats(ScheduleStatsKey key, IEnumerable<DayNumbered> monthDays,
            DepartamentSettings departamentSettings, Schedule? schedule)
        {

            var scheduleStats = new ScheduleStats
            {
                Key = key,
                WorkTimeInMonth = _workTimeService.GetWorkTimeFromDays(monthDays, departamentSettings),
                MonthInQuarter = _calendarService
                    .GetMonthInQuarterNumber(key.Month, departamentSettings.FirstQuarterStart),
                WorkTimeBalance = _workTimeService
                    .GetMonthWorkTimeBalance(schedule?.ScheduleNurses.Count() ?? 0, monthDays, departamentSettings),
                IsClosed = GetIsClosed(schedule),
            };

            scheduleStats.NursesScheduleStats = BuildNursesScheduleStats(monthDays, departamentSettings, schedule);

            return scheduleStats;
        }

        private IEnumerable<NurseScheduleStats> BuildNursesScheduleStats(IEnumerable<DayNumbered> monthDays,
            DepartamentSettings departamentSettings, Schedule? schedule)
        {
            if (schedule is null)
            {
                return Enumerable.Empty<NurseScheduleStats>();
            }

            var nurseScheduleStats = new List<NurseScheduleStats>();

            foreach (var scheduleNurse in schedule.ScheduleNurses)
            {
                nurseScheduleStats.Add(BuildNurseScheduleStats(scheduleNurse, monthDays, departamentSettings));
            }

            return nurseScheduleStats;
        }

        private NurseScheduleStats BuildNurseScheduleStats(ScheduleNurse scheduleNurse, IEnumerable<DayNumbered> monthDays,
            DepartamentSettings departamentSettings)
        {
            var nurseStats = new NurseScheduleStats
            {
                NurseId = scheduleNurse.NurseId,
                AssignedWorkTime = GetAssignedWorkTime(scheduleNurse.NurseWorkDays),
                HolidayHoursAssigned = CalculateHolidayHoursAssigned(scheduleNurse.NurseWorkDays, monthDays, departamentSettings),
                NightHoursAssigned = CalculateNightHoursAssigned(scheduleNurse.NurseWorkDays, monthDays, departamentSettings),
                LastState = GetLastState(scheduleNurse.NurseWorkDays),
                HoursFromLastAssignedShift = _workTimeService.GetHoursFromLastAssignedShift(scheduleNurse.NurseWorkDays),
                HoursToFirstAssignedShift = _workTimeService.GetHoursToFirstAssignedShift(scheduleNurse.NurseWorkDays),
                AssignedMorningShiftsIds = GetAssignedMorningShiftsIds(scheduleNurse.NurseWorkDays),
                WorkTimeAssignedInWeeks = GetWorkTimeInWeeks(scheduleNurse.NurseWorkDays, monthDays),
                TimeOffToAssign = _workTimeService.GetTimeOffTimeToAssign(scheduleNurse.NurseWorkDays, monthDays,
                        departamentSettings),
                TimeOffAssigned = CalculateTimeOffAssigned(scheduleNurse.NurseWorkDays),
            };

            return nurseStats;
        }

        private TimeSpan GetAssignedWorkTime(IEnumerable<NurseWorkDay> nurseWorkDays)
        {
            return nurseWorkDays
                .SumTimeSpan(wd => _workTimeService.GetAssignedShiftWorkTime(wd.ShiftType, wd.MorningShift?.ShiftLength));
        }

        private TimeSpan CalculateHolidayHoursAssigned(IEnumerable<NurseWorkDay> nurseWorkDays,
            IEnumerable<DayNumbered> monthDays, DepartamentSettings departamentSettings)
        {
            return nurseWorkDays.SumTimeSpan(wd => _workTimeService
                .GetShiftHolidayHours(wd.ShiftType, wd.MorningShift?.ShiftLength,
                    monthDays.First(d => d.Date.Day == wd.Day), departamentSettings));
        }

        private TimeSpan CalculateNightHoursAssigned(IEnumerable<NurseWorkDay> nurseWorkDays,
            IEnumerable<DayNumbered> monthDays, DepartamentSettings departamentSettings)
        {
            return nurseWorkDays.SumTimeSpan(wd => _workTimeService
                    .GetShiftNightHours(wd.ShiftType, monthDays.First(d => d.Date.Day == wd.Day), departamentSettings));
        }



        private ScheduleStats GetStatsFromClosedSchedule(ScheduleStatsKey key, Schedule schedule,
            IEnumerable<DayNumbered> days, DepartamentSettings departamentSettings)
        {
            var scheduleStats = new ScheduleStats
            {
                Key = key,
                WorkTimeInMonth = schedule.WorkTimeInMonth,
                WorkTimeBalance = schedule.WorkTimeBalance,
                MonthInQuarter = _calendarService
                    .GetMonthInQuarterNumber(key.Month, departamentSettings.FirstQuarterStart),
                IsClosed = GetIsClosed(schedule),
            };

            var nurseScheduleStats = new List<NurseScheduleStats>();

            foreach (var nurseSchedule in schedule.ScheduleNurses)
            {
                var nurseStats = new NurseScheduleStats
                {
                    NurseId = nurseSchedule.NurseId,
                    AssignedWorkTime = nurseSchedule.AssignedWorkTime,
                    HolidayHoursAssigned = nurseSchedule.HolidaysHoursAssigned,
                    NightHoursAssigned = nurseSchedule.NightHoursAssigned,
                    TimeOffToAssign = nurseSchedule.TimeOffToAssign,
                    TimeOffAssigned = nurseSchedule.TimeOffToAssiged,
                    LastState = GetLastState(nurseSchedule.NurseWorkDays),
                    HoursFromLastAssignedShift = _workTimeService.GetHoursFromLastAssignedShift(nurseSchedule.NurseWorkDays),
                    HoursToFirstAssignedShift = _workTimeService.GetHoursToFirstAssignedShift(nurseSchedule.NurseWorkDays),
                    AssignedMorningShiftsIds = GetAssignedMorningShiftsIds(nurseSchedule.NurseWorkDays),
                    WorkTimeAssignedInWeeks = GetWorkTimeInWeeks(nurseSchedule.NurseWorkDays, days),
                };

                nurseScheduleStats.Add(nurseStats);
            }

            scheduleStats.NursesScheduleStats = nurseScheduleStats;

            return scheduleStats;
        }

        private IEnumerable<int> GetAssignedMorningShiftsIds(IEnumerable<NurseWorkDay> nurseWorkDays)
        {
            return nurseWorkDays
                .Where(d => d.ShiftType == ShiftTypes.Morning)
                .Select(d => d.MorningShift.MorningShiftId);
        }

        private ShiftTypes GetLastState(IEnumerable<NurseWorkDay> nurseWorkDays)
        {
            return nurseWorkDays.First(d => d.Day == nurseWorkDays.Max(d => d.Day)).ShiftType;
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

                var workDay = nurseWorkDays.First(d => d.Day == day.Date.Day);

                workTimeInWeeks[day.WeekInQuarter] += _workTimeService.GetAssignedShiftWorkTime(workDay.ShiftType, 
                    workDay.MorningShift?.ShiftLength);
            }

            return workTimeInWeeks;
        }

        private TimeSpan CalculateTimeOffAssigned(IEnumerable<NurseWorkDay> nurseWorkDays)
        {
            return nurseWorkDays
                .Where(d => d.IsTimeOff == true)
                .SumTimeSpan(wd => _workTimeService.GetAssignedShiftWorkTime(wd.ShiftType, wd.MorningShift?.ShiftLength));
        }
        private bool GetIsClosed(Schedule? schedule)
        {
            return schedule is not null && schedule.IsClosed;
        }
    }
}

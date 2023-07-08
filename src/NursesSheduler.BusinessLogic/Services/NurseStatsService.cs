using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Services
{
    internal sealed class NurseStatsService : INurseStatsService
    {
        private readonly IApplicationDbContext _context;
        private readonly ICalendarService _calendarService;

        public NurseStatsService(IApplicationDbContext context, ICalendarService calendarService)
        {
            _context = context;
            _calendarService = calendarService;
        }

        public async Task<ICollection<NurseQuarterStats>> GetNurseQuarterStats(Schedule currentSchedule,
            DepartamentSettings departamentSettings)
        {
            var nurseQuarterStats = new List<NurseQuarterStats>();
            foreach (var scheduleNurses in currentSchedule.ScheduleNurses)
            {
                nurseQuarterStats.Add(new NurseQuarterStats(scheduleNurses.NurseId));
            }

            var quarterSchedules = await _context.Schedules
                .Include(s => s.ScheduleNurses)
                .ThenInclude(n => n.NurseWorkDays)
                .ThenInclude(d => d.MorningShift)
                .Where(s => s.Quarter.QuarterId == currentSchedule.Quarter.QuarterId &&
                    s.MonthInQuarter != currentSchedule.MonthInQuarter)
                .ToListAsync();

            quarterSchedules.Add(currentSchedule);

            var quarterMonthsDates = _calendarService
                .GetMonthsInQuarterDates(departamentSettings.FirstQuarterStart, currentSchedule.Quarter.QuarterNumber,
                    currentSchedule.Year);

            foreach (var monthDate in quarterMonthsDates)
            {
                var daysInMonth = await _calendarService.GetMonthDays(monthDate.Month, monthDate.Year, 
                    departamentSettings.FirstQuarterStart);

                var schedule = quarterSchedules.FirstOrDefault(s => s.MonthNumber == monthDate.Month);

                PopulateMonthNurseQuarterStats(nurseQuarterStats, schedule, daysInMonth, departamentSettings);
            }

            return nurseQuarterStats;
        }

        private void PopulateMonthNurseQuarterStats(ICollection<NurseQuarterStats> nurseQuarterStats,
            Schedule? currentSchedule, Day[] daysInMonth, DepartamentSettings departamentSettings)
        {
            foreach (var day in daysInMonth)
            {
                foreach (var nurseStats in nurseQuarterStats)
                {
                    if (nurseStats.WorkTimeAssignedInWeeks.FirstOrDefault(t => t.WeekNumber == day.WeekInQuarter)
                        is null)
                    {
                        nurseStats.WorkTimeAssignedInWeeks.Add(new WorkTimeInWeek
                        {
                            WeekNumber = day.WeekInQuarter,
                            AssignedWorkTime = TimeSpan.Zero,
                        });
                    }

                    var currentWorkDay = currentSchedule?
                        .ScheduleNurses?
                        .FirstOrDefault(n => n.NurseId == nurseStats.NurseId)?
                        .NurseWorkDays?
                        .FirstOrDefault(d => d.DayNumber == day.Date.DayNumber);

                    if (currentWorkDay is not null)
                    {
                        if (currentWorkDay.ShiftType == Domain.Enums.ShiftTypes.Night)
                        {
                            nurseStats.NumberOfNightShifts++;

                            if (!day.IsWorkingDay)
                            {
                                nurseStats.HolidayPaidHoursAssigned += departamentSettings
                                    .NightShiftHolidayEligibleHours;
                            }

                            nurseStats.WorkTimeAssignedInWeeks
                                .First(t => t.WeekNumber == day.WeekInQuarter).AssignedWorkTime
                                    += GeneralConstants.RegularShiftLenght;
                            nurseStats.WorkTimeAssignedInQuarter += GeneralConstants.RegularShiftLenght;
                        }
                        else if (currentWorkDay.ShiftType == Domain.Enums.ShiftTypes.Day)
                        {
                            if (!day.IsWorkingDay)
                            {
                                nurseStats.HolidayPaidHoursAssigned += departamentSettings
                                    .DayShiftHolidayEligibleHours;
                            }

                            nurseStats.WorkTimeAssignedInWeeks
                                .First(t => t.WeekNumber == day.WeekInQuarter).AssignedWorkTime
                                    += GeneralConstants.RegularShiftLenght;
                            nurseStats.WorkTimeAssignedInQuarter += GeneralConstants.RegularShiftLenght;
                        }
                        else if (currentWorkDay.ShiftType == Domain.Enums.ShiftTypes.Morning)
                        {
                            if (!day.IsWorkingDay)
                            {
                                nurseStats.HolidayPaidHoursAssigned += currentWorkDay.MorningShift.ShiftLength;
                            }

                            nurseStats.WorkTimeAssignedInWeeks
                                .First(t => t.WeekNumber == day.WeekInQuarter).AssignedWorkTime
                                    += currentWorkDay.MorningShift.ShiftLength;

                            nurseStats.MorningShiftsAssigned.Add(currentWorkDay.MorningShift);
                            nurseStats.WorkTimeAssignedInQuarter += currentWorkDay.MorningShift.ShiftLength;
                        }
                    }
                }
            }
        }
    }
}

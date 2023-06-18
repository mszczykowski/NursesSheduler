﻿using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.BusinessLogic.Abstractions.Solver.StateManagers;
using NursesScheduler.BusinessLogic.Solver.StateManagers;
using NursesScheduler.Domain;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Services
{
    internal sealed class SchedulesService : ISchedulesService
    {
        private readonly IApplicationDbContext _context;
        private readonly IWorkTimeService _workTimeService;
        private readonly ICalendarService _calendarService;

        public SchedulesService(IApplicationDbContext context, IWorkTimeService workTimeService, 
            ICalendarService calendarService)
        {
            _context = context;
            _workTimeService = workTimeService;
            _calendarService = calendarService;
        }

        public async Task<Schedule> GetNewSchedule(int monthNumber, int yearNumber, int departamentId, 
            DepartamentSettings departamentSettings)
        {
            var quarterNumber = _calendarService.GetQuarterNumber(monthNumber, departamentSettings.FirstQuarterStart);
            var quarterMonthDates = _calendarService.GetMonthsInQuarterDates(departamentSettings.FirstQuarterStart,
                quarterNumber, yearNumber);

            var monthInQuarter = 1;
            foreach(var quarterMonthDate in quarterMonthDates)
            {
                if(quarterMonthDate.Month < monthNumber)
                {
                    monthInQuarter++;
                }
            }

            var nurses = await _context.Nurses
                .Where(n => n.IsDeleted == false && n.DepartamentId == departamentId)
                .ToListAsync();

            var schedule = new Schedule
            {
                MonthNumber = monthNumber,
                Year = yearNumber,
                DepartamentId = departamentId,
                TimeOffAssigned = TimeSpan.Zero,
                MonthInQuarter = monthInQuarter,
            };

            schedule.Quarter = await GetQuarter(yearNumber, quarterNumber, departamentId, departamentSettings);

            schedule.WorkTimeInMonth = await _workTimeService
                .GetTotalWorkingHoursInMonth(monthNumber, yearNumber, departamentSettings.WorkingTime);

            schedule.TimeOffAvailableToAssgin = await _workTimeService
                .GetSurplusWorkTime(monthNumber, yearNumber, nurses.Count, departamentSettings);

            InitialiseScheduleNurses(nurses, schedule, DateTime.DaysInMonth(yearNumber, monthNumber));

            return schedule;
        }

        public async Task SetTimeOffs(Schedule schedule, DepartamentSettings departamentSettings)
        {
            var absenceSummaries = await _context.AbsencesSummaries
                .Where(a => a.Year == schedule.Year &&
                    a.Nurse.DepartamentId == schedule.DepartamentId)
                .Include(a => a.Absences)
                .ToListAsync();

            if (absenceSummaries == null || !absenceSummaries.Any())
                return;

            foreach(var scheduleNurse in schedule.ScheduleNurses)
            {
                var absences = absenceSummaries
                    .FirstOrDefault(a => a.NurseId == scheduleNurse.NurseId && a.Year == schedule.Year)?
                    .Absences?.Where(a => a.Month == schedule.MonthNumber)
                    .ToList();

                if (absences == null)
                    continue;

                foreach(var absence in absences)
                {
                    foreach(var day in absence.Days)
                    {
                        scheduleNurse.NurseWorkDays.First(d => d.DayNumber == day).IsTimeOff = true;
                    }
                    scheduleNurse.TimeOffToAssign += absence.WorkTimeToAssign;
                }
            }
        }

        private void InitialiseScheduleNurses(ICollection<Nurse> nurses, Schedule schedule, int numberOfDaysInMonth)
        {
            schedule.ScheduleNurses = new List<ScheduleNurse>();

            foreach (var nurse in nurses)
            {
                var scheduleNurse = new ScheduleNurse
                {
                    NurseId = nurse.NurseId,
                    Nurse = nurse,
                    NurseWorkDays = new List<NurseWorkDay>(),
                    TimeToAssingInMonth = schedule.WorkTimeInMonth,
                };

                for (int i = 0; i < numberOfDaysInMonth; i++)
                {
                    scheduleNurse.NurseWorkDays.Add(new NurseWorkDay
                    {
                        DayNumber = i + 1,
                        ShiftType = Domain.Enums.ShiftTypes.None,
                    });
                }
                schedule.ScheduleNurses.Add(scheduleNurse);
            }
        }

        private async Task<Quarter> GetQuarter(int yearNumber, int quarterNumber, int departamentId,
            DepartamentSettings departamentSettings)
        {
            var quarter = await _context.Quarters
                .Include(q => q.MorningShifts)
                .FirstOrDefaultAsync(q => q.DepartamentId == departamentId
                                        && q.SettingsVersion == departamentSettings.SettingsVersion
                                        && q.QuarterNumber == quarterNumber
                                        && q.QuarterYear == yearNumber);

            if (quarter == null)
            {
                quarter = await CreateNewQuarter(yearNumber, quarterNumber, departamentId, departamentSettings);
            }

            return quarter;
        }

        private async Task<Quarter> CreateNewQuarter(int yearNumber, int quarterNumber, int departamentId,
            DepartamentSettings settings)
        {
            var quarter = new Quarter
            {
                QuarterYear = yearNumber,
                QuarterNumber = quarterNumber,
                DepartamentId = departamentId,
                SettingsVersion = settings.SettingsVersion,
            };
            quarter.MorningShifts = new List<MorningShift>();
            quarter.WorkTimeInQuarterToAssign = await _workTimeService
                        .GetTotalWorkingHoursInQuarter(quarter.QuarterNumber, quarter.QuarterYear, settings);

            return quarter;
        }

        public async Task SetNurseWorkTimes(Schedule currentSchedule)
        {
            var previousSchedules = await _context.Schedules
                .Where(s => s.Quarter == currentSchedule.Quarter &&
                    s.MonthNumber != currentSchedule.MonthNumber &&
                    s.Year == currentSchedule.Year &&
                    s.DepartamentId == currentSchedule.DepartamentId)
                .Include(s => s.ScheduleNurses)
                .ThenInclude(n => n.NurseWorkDays)
                .OrderBy(s => s.MonthInQuarter)
                .ToListAsync();

            foreach(var nurse in currentSchedule.ScheduleNurses)
            {
                nurse.TimeToAssingInQuarterLeft = currentSchedule.Quarter.WorkTimeInQuarterToAssign;
            }

            for(int i = 0; i < previousSchedules.Count; i++)
            {
                foreach (var nurse in previousSchedules[i].ScheduleNurses)
                {
                    nurse.TimeToAssingInMonth = previousSchedules[i].WorkTimeInMonth - 
                        _workTimeService.GetWorkingTimeFromWorkDays(nurse.NurseWorkDays);

                    var currentNurseStats = currentSchedule.ScheduleNurses
                        .FirstOrDefault(n => n.NurseId == nurse.NurseId);

                    if (currentNurseStats != null)
                        currentNurseStats.TimeToAssingInQuarterLeft -= 
                            _workTimeService.GetWorkingTimeFromWorkDays(nurse.NurseWorkDays);

                    ScheduleNurse? prevNurseStats = null;

                    if(i - 1 >= 0)
                    {
                        prevNurseStats = previousSchedules[i - 1].ScheduleNurses
                            .FirstOrDefault(nurse => nurse.NurseId == nurse.NurseId);
                    }

                    if (prevNurseStats == null)
                    {
                        nurse.PreviousMonthTime = TimeSpan.Zero;
                    }
                    else
                    {
                        nurse.PreviousMonthTime = prevNurseStats.TimeToAssingInMonth + prevNurseStats.PreviousMonthTime;
                    }

                }
            }

            foreach (var nurse in currentSchedule.ScheduleNurses)
            {
                nurse.TimeToAssingInMonth = currentSchedule.WorkTimeInMonth - 
                    _workTimeService.GetWorkingTimeFromWorkDays(nurse.NurseWorkDays);
                
                nurse.TimeToAssingInQuarterLeft -= _workTimeService.GetWorkingTimeFromWorkDays(nurse.NurseWorkDays);

                ScheduleNurse? prevNurseStats = null;

                if (!previousSchedules.Any())
                    continue;

                prevNurseStats = previousSchedules.Last()?.ScheduleNurses
                    .FirstOrDefault(nurse => nurse.NurseId == nurse.NurseId);

                if (prevNurseStats == null)
                {
                    nurse.PreviousMonthTime = TimeSpan.Zero;
                }
                else
                {
                    nurse.PreviousMonthTime = prevNurseStats.TimeToAssingInMonth + prevNurseStats.PreviousMonthTime;
                } 
            }
        }

        public async Task<Schedule> GenerateSchedule(Schedule initialState, DepartamentSettings departamentSettings)
        {
            var nurseQuarterStats = GetNurseQuarterStats(initialState, departamentSettings);
            var previousSchedule = GetPreviousSchedule(initialState);

            var nurseStates = new HashSet<INurseState>();

            foreach(var quarterStats in nurseQuarterStats)
            {
                nurseStates.Add(new NurseState())
            }
        }


        private async Task<ICollection<NurseQuarterStats>> GetNurseQuarterStats(Schedule initialState,
            DepartamentSettings departamentSettings)
        {
            var nurseQuarterStats = new List<NurseQuarterStats>();
            foreach (var scheduleNurses in initialState.ScheduleNurses)
            {
                nurseQuarterStats.Add(new NurseQuarterStats(scheduleNurses.NurseId));
            }

            var quarterSchedules = await _context.Schedules
                .Include(s => s.ScheduleNurses)
                .ThenInclude(n => n.NurseWorkDays)
                .ThenInclude(d => d.MorningShift)
                .Where(s => s.Quarter.QuarterId == initialState.Quarter.QuarterId &&
                    s.MonthInQuarter != initialState.MonthInQuarter)
                .ToListAsync();

            quarterSchedules.Add(initialState);

            await PopulateNurseQuarterStats(nurseQuarterStats, quarterSchedules, departamentSettings);

            return nurseQuarterStats;
        }

        private async Task PopulateNurseQuarterStats(ICollection<NurseQuarterStats> nurseQuarterStats, 
            ICollection<Schedule> quarterSchedules, DepartamentSettings departamentSettings)
        {
            var schedule = quarterSchedules.First();

            var quarterMonthsDates = _calendarService
                .GetMonthsInQuarterDates(departamentSettings.FirstQuarterStart, schedule.Quarter.QuarterNumber, 
                    schedule.Year);
            
            foreach(var monthDate in quarterMonthsDates)
            {
                var currentSchedule = quarterSchedules
                    .FirstOrDefault(s => s.MonthNumber == monthDate.Month &&
                                        s.Year == monthDate.Year);

                var daysInMonth = await _calendarService
                    .GetMonthDays(monthDate.Month, monthDate.Year,
                    departamentSettings.FirstQuarterStart);

                foreach(var day in daysInMonth)
                {
                    foreach (var nurseStats in nurseQuarterStats)
                    {
                        if(nurseStats.WorkTimeAssignedInWeek.FirstOrDefault(t => t.WeekNumber == day.WeekInQuarter) 
                            is null)
                        {
                            nurseStats.WorkTimeAssignedInWeek.Add(new WorkTimeInWeek
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

                        if(currentWorkDay != null)
                        {
                            if(currentWorkDay.ShiftType == Domain.Enums.ShiftTypes.Night)
                            {
                                nurseStats.NumberOfNightShifts++;

                                if(!day.IsWorkingDay)
                                {
                                    nurseStats.HolidayPaidHoursAssigned += departamentSettings
                                        .NightShiftHolidayEligibleHours;
                                }

                                nurseStats.WorkTimeAssignedInWeek
                                    .First(t => t.WeekNumber == day.WeekInQuarter).AssignedWorkTime
                                        += GeneralConstants.RegularShiftLenght;
                            }
                            else if (currentWorkDay.ShiftType == Domain.Enums.ShiftTypes.Day)
                            {
                                if (!day.IsWorkingDay)
                                {
                                    nurseStats.HolidayPaidHoursAssigned += departamentSettings
                                        .DayShiftHolidayEligibleHours;
                                }

                                nurseStats.WorkTimeAssignedInWeek
                                    .First(t => t.WeekNumber == day.WeekInQuarter).AssignedWorkTime
                                        += GeneralConstants.RegularShiftLenght;
                            }
                            else if (currentWorkDay.ShiftType == Domain.Enums.ShiftTypes.Morning)
                            {
                                if (!day.IsWorkingDay)
                                {
                                    nurseStats.HolidayPaidHoursAssigned += currentWorkDay.MorningShift.ShiftLength;
                                }

                                nurseStats.WorkTimeAssignedInWeek
                                    .First(t => t.WeekNumber == day.WeekInQuarter).AssignedWorkTime
                                        += currentWorkDay.MorningShift.ShiftLength;

                                nurseStats.MorningShiftsAssigned.Add(currentWorkDay.MorningShift);
                            }
                        }
                    }
                }
            } 
        }


        private async Task<Schedule?> GetPreviousSchedule(Schedule currentSchedule)
        {
            if (currentSchedule == null)
            {
                return null;
            }

            var prevMonth = currentSchedule.MonthNumber - 1;
            var prevYear = currentSchedule.Year;
            if (prevMonth <= 0)
            {
                prevMonth = 12;
                prevYear--;
            }

            return await _context.Schedules
                .Include(s => s.ScheduleNurses)
                .ThenInclude(n => n.NurseWorkDays)
                .FirstOrDefaultAsync(s => s.DepartamentId == currentSchedule.DepartamentId &&
                    s.Year == prevYear &&
                    s.MonthNumber == prevMonth);
        }
    }
}

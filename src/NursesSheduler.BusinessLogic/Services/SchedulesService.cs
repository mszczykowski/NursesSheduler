using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Services
{
    internal sealed class SchedulesService : ISchedulesService
    {
        private readonly IApplicationDbContext _context;
        private readonly IWorkTimeService _workTimeService;
        private readonly ICalendarService _calendarService;
        private readonly INursesService _nursesService;

        public SchedulesService(IApplicationDbContext context, IWorkTimeService workTimeService,
            ICalendarService calendarService, INursesService nursesService)
        {
            _context = context;
            _workTimeService = workTimeService;
            _calendarService = calendarService;
            _nursesService = nursesService;
        }

        public async Task<Schedule> GetNewSchedule(int monthNumber, int yearNumber, int departamentId,
            DepartamentSettings departamentSettings)
        {
            var quarterNumber = _calendarService.GetQuarterNumber(monthNumber, departamentSettings.FirstQuarterStart);
            var quarterMonthDates = _calendarService.GetMonthsInQuarterDatesAsync(departamentSettings.FirstQuarterStart,
                quarterNumber, yearNumber);

            var monthInQuarter = 1;
            foreach (var quarterMonthDate in quarterMonthDates)
            {
                if (quarterMonthDate.Month < monthNumber)
                {
                    monthInQuarter++;
                }
            }

            var nurses = await _nursesService.GetActiveDepartamentNurses(departamentId);

            var schedule = new Schedule
            {
                Month = monthNumber,
                Year = yearNumber,
                TimeOffAssigned = TimeSpan.Zero,
                MonthInQuarter = monthInQuarter,
            };

            schedule.Quarter = await GetQuarter(yearNumber, quarterNumber, departamentId, departamentSettings);

            schedule.WorkTimeInMonth = await _workTimeService
                .GetTotalWorkingHoursInMonth(monthNumber, yearNumber, departamentSettings.WorkDayLength);

            schedule.WorkTimeBalance = await _workTimeService
                .GetSurplusWorkTime(monthNumber, yearNumber, nurses.Count(), departamentSettings);

            InitialiseScheduleNurses(nurses, schedule, DateTime.DaysInMonth(yearNumber, monthNumber));

            return schedule;
        }

        public async Task SetTimeOffs(Schedule schedule, DepartamentSettings departamentSettings)
        {
            var absenceSummaries = await _context.AbsencesSummaries
                .Where(a => a.Year == schedule.Year &&
                    a.Nurse.DepartamentId == schedule.Quarter.DepartamentId)
                .Include(a => a.Absences)
                .ToListAsync();

            if (absenceSummaries == null || !absenceSummaries.Any())
                return;

            foreach (var scheduleNurse in schedule.ScheduleNurses)
            {
                var absences = absenceSummaries
                    .FirstOrDefault(a => a.NurseId == scheduleNurse.NurseId && a.Year == schedule.Year)?
                    .Absences?.Where(a => a.MonthNumber == schedule.Month)
                    .ToList();

                if (absences == null)
                    continue;

                foreach (var absence in absences)
                {
                    foreach (var day in absence.Days)
                    {
                        scheduleNurse.NurseWorkDays.First(d => d.DayNumber == day).IsTimeOff = true;
                    }
                    scheduleNurse.TimeOffToAssign += absence.WorkTimeToAssign;
                }
            }
        }

        private void InitialiseScheduleNurses(IEnumerable<Nurse> nurses, Schedule schedule, int numberOfDaysInMonth)
        {
            schedule.ScheduleNurses = new List<ScheduleNurse>();

            foreach (var nurse in nurses)
            {
                schedule.ScheduleNurses.Add(GetScheduleNurse(nurse, numberOfDaysInMonth));
            }
        }

        private ScheduleNurse GetScheduleNurse(Nurse nurse, int numberOfDaysInMonth)
        {
            var scheduleNurse = new ScheduleNurse
            {
                NurseId = nurse.NurseId,
                Nurse = nurse,
                NurseWorkDays = new List<NurseWorkDay>(),
            };

            for (int i = 0; i < numberOfDaysInMonth; i++)
            {
                scheduleNurse.NurseWorkDays.Add(new NurseWorkDay
                {
                    DayNumber = i + 1,
                    ShiftType = Domain.Enums.ShiftTypes.None,
                });
            }

            return scheduleNurse;
        }

        private async Task<Quarter> GetQuarter(int yearNumber, int quarterNumber, int departamentId,
            DepartamentSettings departamentSettings)
        {
            var quarter = await _context.Quarters
                .Include(q => q.MorningShifts)
                .FirstOrDefaultAsync(q => q.DepartamentId == departamentId
                                        && q.SettingsVersion == departamentSettings.SettingsVersion
                                        && q.QuarterNumber == quarterNumber
                                        && q.Year == yearNumber);

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
                Year = yearNumber,
                QuarterNumber = quarterNumber,
                DepartamentId = departamentId,
                SettingsVersion = settings.SettingsVersion,
            };
            quarter.MorningShifts = new List<MorningShift>();
            quarter.WorkTimeInQuarterToAssign = await _workTimeService
                        .GetTotalWorkingHoursInQuarter(quarter.QuarterNumber, quarter.Year, settings);

            return quarter;
        }

        public async Task CalculateNurseWorkTimes(Schedule currentSchedule)
        {
            var previousSchedules = await _context.Schedules
                .Where(s => s.QuarterId == currentSchedule.QuarterId &&
                    s.Month != currentSchedule.Month &&
                    s.Year == currentSchedule.Year)
                .Include(s => s.ScheduleNurses)
                .ThenInclude(n => n.NurseWorkDays)
                .OrderBy(s => s.MonthInQuarter)
                .ToListAsync();

            foreach (var nurse in currentSchedule.ScheduleNurses)
            {
                nurse.TimeToAssingInQuarterLeft = currentSchedule.Quarter.WorkTimeInQuarterToAssign;
            }

            for (int i = 0; i < previousSchedules.Count; i++)
            {
                foreach (var nurse in previousSchedules[i].ScheduleNurses)
                {
                    nurse.AssignedWorkTime = previousSchedules[i].WorkTimeInMonth -
                        _workTimeService.GetWorkDayLengthFromWorkDays(nurse.NurseWorkDays);

                    var currentNurseStats = currentSchedule.ScheduleNurses
                        .FirstOrDefault(n => n.NurseId == nurse.NurseId);

                    if (currentNurseStats != null)
                        currentNurseStats.TimeToAssingInQuarterLeft -=
                            _workTimeService.GetWorkDayLengthFromWorkDays(nurse.NurseWorkDays);

                    ScheduleNurse? prevNurseStats = null;

                    if (i - 1 >= 0)
                    {
                        prevNurseStats = previousSchedules[i - 1].ScheduleNurses
                            .FirstOrDefault(nurse => nurse.NurseId == nurse.NurseId);
                    }

                    if (prevNurseStats == null)
                    {
                        nurse.PreviousMonthBalance = TimeSpan.Zero;
                    }
                    else
                    {
                        nurse.PreviousMonthBalance = prevNurseStats.AssignedWorkTime + prevNurseStats.PreviousMonthBalance;
                    }

                }
            }

            foreach (var nurse in currentSchedule.ScheduleNurses)
            {
                nurse.AssignedWorkTime = currentSchedule.WorkTimeInMonth -
                    _workTimeService.GetWorkDayLengthFromWorkDays(nurse.NurseWorkDays);

                nurse.TimeToAssingInQuarterLeft -= _workTimeService.GetWorkDayLengthFromWorkDays(nurse.NurseWorkDays);

                ScheduleNurse? prevNurseStats = null;

                if (!previousSchedules.Any())
                    continue;

                prevNurseStats = previousSchedules.Last()?.ScheduleNurses
                    .FirstOrDefault(nurse => nurse.NurseId == nurse.NurseId);

                if (prevNurseStats == null)
                {
                    nurse.PreviousMonthBalance = TimeSpan.Zero;
                }
                else
                {
                    nurse.PreviousMonthBalance = prevNurseStats.AssignedWorkTime + prevNurseStats.PreviousMonthBalance;
                }
            }
        }

        public async Task<Schedule?> GetPreviousSchedule(Schedule currentSchedule)
        {
            if (currentSchedule == null)
            {
                return null;
            }

            var prevMonth = new DateOnly(currentSchedule.Year, currentSchedule.Month, 1).AddMonths(-1);

            return await _context.Schedules
                .Include(s => s.ScheduleNurses)
                .ThenInclude(n => n.NurseWorkDays)
                .FirstOrDefaultAsync(s => s.Quarter.DepartamentId == currentSchedule.Quarter.DepartamentId &&
                    s.Year == prevMonth.Year &&
                    s.Month == prevMonth.Month);
        }

        public async Task UpdateScheduleNurses(Schedule schedule)
        {
            var activeNurses = await _nursesService.GetActiveDepartamentNurses(schedule.Quarter.DepartamentId);
            var numberOfDaysInMonth = DateTime.DaysInMonth(schedule.Year, schedule.Month);

            foreach (var nurse in activeNurses)
            {
                if (schedule.ScheduleNurses.Any(n => n.NurseId == nurse.NurseId))
                {
                    continue;
                }

                schedule.ScheduleNurses.Add(GetScheduleNurse(nurse, numberOfDaysInMonth));
            }

            foreach (var scheduleNurse in schedule.ScheduleNurses)
            {
                if (activeNurses.Any(n => n.NurseId == scheduleNurse.NurseId))
                {
                    continue;
                }

                schedule.ScheduleNurses.Remove(scheduleNurse);
            }
        }

        public async Task RecalculateScheduleNurse(ScheduleNurse scheduleNurse, TimeSpan workTimeInQuarter,
            TimeSpan workTimeInMonth, IEnumerable<Day> days, IEnumerable<MorningShift> morningShifts)
        {
            //    public TimeSpan PreviousMonthTime { get; set; }
            //public TimeSpan TimeToAssingInMonth { get; set; }
            //public TimeSpan TimeOffToAssign { get; set; }
            //public TimeSpan TimeToAssingInQuarterLeft { get; set; }
            //public TimeSpan HolidaysHoursAssigned { get; set; }
            //public int NightShiftsAssigned { get; set; }

            scheduleNurse.AssignedWorkTime = workTimeInMonth;
            scheduleNurse.T
        }
    }
}

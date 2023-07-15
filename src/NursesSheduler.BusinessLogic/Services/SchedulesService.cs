using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Enums;
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

        public async Task<Schedule> CreateNewScheduleAsync(int month, Quarter quarter)
        {
            return new Schedule
            {
                Month = month,
                IsClosed = false,
                QuarterId = quarter.QuarterId,
                ScheduleNurses = await InitializeScheduleNursesAsync(quarter.Year, month, quarter.DepartamentId),
            };
        }

        private async Task<IEnumerable<ScheduleNurse>> InitializeScheduleNursesAsync(int year, int month,
            int departamentId)
        {
            var scheduleNurses = new List<ScheduleNurse>();
            var nurses = await _nursesService.GetActiveDepartamentNurses(departamentId);

            foreach (var nurse in nurses)
            {
                scheduleNurses.Add(new ScheduleNurse
                {
                    NurseId = nurse.NurseId,
                    NurseWorkDays = InitializeWorkDays(year, month),
                });
            }

            return scheduleNurses;
        }

        private IEnumerable<NurseWorkDay> InitializeWorkDays(int year, int month)
        {
            var workDays = new List<NurseWorkDay>();

            for(int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
            {
                workDays.Add(new NurseWorkDay
                {
                    Day = i,
                    ShiftType = ShiftTypes.None,
                });
            }

            return workDays;
        }

        public async Task SetTimeOffsAsync(Schedule schedule, DepartamentSettings departamentSettings)
        {
            throw new NotImplementedException();

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

        public async Task UpdateScheduleNurses(Schedule schedule)
        {
            throw new NotImplementedException();

            var nurses = await _nursesService.GetActiveDepartamentNurses(schedule.Quarter.DepartamentId);

            foreach (var nurse in nurses)
            {
                if (schedule.ScheduleNurses.Any(n => n.NurseId == nurse.NurseId))
                {
                    continue;
                }

                schedule.ScheduleNurses.Add(GetScheduleNurse(nurse, numberOfDaysInMonth));
            }

            foreach (var scheduleNurse in schedule.ScheduleNurses)
            {
                if (nurses.Any(n => n.NurseId == scheduleNurse.NurseId))
                {
                    continue;
                }

                schedule.ScheduleNurses.Remove(scheduleNurse);
            }
        }
    }
}

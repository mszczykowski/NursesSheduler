using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure.Providers;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Enums;
using NursesScheduler.Domain.Exceptions;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.Services
{
    internal sealed class SchedulesService : ISchedulesService
    {
        private readonly IActiveNursesService _activeNursesService;
        private readonly IAbsencesService _absencesService;
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IScheduleStatsProvider _scheduleStatsProvider;

        public SchedulesService(IActiveNursesService activeNursesService, IApplicationDbContext applicationDbContext,
            IAbsencesService absencesService, IScheduleStatsProvider scheduleStatsProvider)
        {
            _activeNursesService = activeNursesService;
            _absencesService = absencesService;
            _applicationDbContext = applicationDbContext;
            _scheduleStatsProvider = scheduleStatsProvider;
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

        public async Task SetTimeOffsAsync(int year, int month, Schedule schedule)
        {
            foreach(var schdeuleNurse in schedule.ScheduleNurses)
            {
                await SetTimeOffsAsync(year, month, schdeuleNurse);
            }
        }

        private async Task SetTimeOffsAsync(int year, int month, ScheduleNurse scheduleNurse)
        {
            var absences = await _absencesService.GetNurseAbsencesInMonthAsync(year, month, scheduleNurse.NurseId);

            foreach (var absence in absences)
            {
                foreach (var day in absence.Days)
                {
                    scheduleNurse.NurseWorkDays.First(d => d.Day == day).IsTimeOff = true;
                }
            }
        }

        public void ResolveMorningShifts(Schedule schedule, IEnumerable<MorningShift> morningShifts)
        {
            foreach(var nurseWorkDay in schedule.ScheduleNurses.SelectMany(s => s.NurseWorkDays))
            {
                if(nurseWorkDay.ShiftType == ShiftTypes.Morning)
                {
                    nurseWorkDay.MorningShift = morningShifts.First(m => m.MorningShiftId == nurseWorkDay.MorningShiftId);
                }
            }
        }

        public async Task<int> UpsertSchedule(Schedule updatedSchdeule, CancellationToken cancellationToken)
        {
            var quarter = await _applicationDbContext.Quarters.FindAsync(updatedSchdeule.QuarterId)
                ?? throw new EntityNotFoundException(updatedSchdeule.QuarterId, nameof(Quarter));

            _scheduleStatsProvider.InvalidateCache(new ScheduleStatsKey
            {
                DepartamentId = quarter.DepartamentId,
                Month = updatedSchdeule.Month,
                Year = quarter.Year,
            });

            var oldSchedule = await _applicationDbContext.Schedules
                .Include(s => s.ScheduleNurses)
                .ThenInclude(n => n.NurseWorkDays)
                .FirstOrDefaultAsync(s => s.QuarterId == updatedSchdeule.QuarterId &&
                    s.Month == updatedSchdeule.Month);

            if (oldSchedule is null)
            {
                return await AddSchedule(updatedSchdeule, cancellationToken);
            }

            return await EditSchedule(oldSchedule, updatedSchdeule, cancellationToken);
        }

        private async Task<int> AddSchedule(Schedule newSchedule, CancellationToken cancellationToken)
        {
            _applicationDbContext.Schedules.Add(newSchedule);

            return await _applicationDbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task<int> EditSchedule(Schedule oldSchedule, Schedule updatedSchedule, CancellationToken cancellationToken)
        {
            oldSchedule.IsClosed = updatedSchedule.IsClosed;

            foreach (var oldScheduleNurse in oldSchedule.ScheduleNurses)
            {
                var updatedWorkDays = updatedSchedule
                    .ScheduleNurses
                    .First(n => n.NurseId == oldScheduleNurse.NurseId)
                    .NurseWorkDays;

                foreach (var oldWorkDay in oldScheduleNurse.NurseWorkDays)
                {
                    var updatedWorkDay = updatedWorkDays.First(wd => wd.Day == oldWorkDay.Day);

                    oldWorkDay.ShiftType = updatedWorkDay.ShiftType;

                    if (updatedWorkDay.ShiftType == Domain.Enums.ShiftTypes.Morning)
                    {
                        oldWorkDay.MorningShiftId = updatedWorkDay.MorningShiftId;
                        continue;
                    }

                    oldWorkDay.MorningShift = null;
                    oldWorkDay.MorningShiftId = null;
                }
            }

            return await _applicationDbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task<IEnumerable<ScheduleNurse>> InitializeScheduleNursesAsync(int year, int month,
            int departamentId)
        {
            var scheduleNurses = new List<ScheduleNurse>();
            var nurses = await _activeNursesService.GetActiveDepartamentNurses(departamentId);

            foreach (var nurse in nurses)
            {
                var scheduleNurse = new ScheduleNurse
                {
                    NurseId = nurse.NurseId,
                    NurseWorkDays = InitializeWorkDays(year, month),
                };

                await SetTimeOffsAsync(year, month, scheduleNurse);

                scheduleNurses.Add(scheduleNurse);
            }

            return scheduleNurses;
        }

        private IEnumerable<NurseWorkDay> InitializeWorkDays(int year, int month)
        {
            var workDays = new List<NurseWorkDay>();

            for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
            {
                workDays.Add(new NurseWorkDay
                {
                    Day = i,
                    ShiftType = ShiftTypes.None,
                });
            }

            return workDays;
        }
    }
}

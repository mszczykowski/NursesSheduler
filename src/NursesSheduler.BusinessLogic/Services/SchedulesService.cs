using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.Services
{
    internal sealed class SchedulesService : ISchedulesService
    {
        private readonly IActiveNursesService _activeNursesService;
        private readonly IAbsencesService _absencesService;

        public SchedulesService(IActiveNursesService activeNursesService, IAbsencesService absencesService)
        {
            _activeNursesService = activeNursesService;
            _absencesService = absencesService;
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

        public async Task SetTimeOffsAsync(int year, int month, ScheduleNurse scheduleNurse)
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

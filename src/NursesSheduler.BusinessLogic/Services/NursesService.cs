using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Services
{
    internal sealed class NursesService : INursesService
    {
        private readonly IApplicationDbContext _context;

        public NursesService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Nurse>> GetActiveDepartamentNurses(int departamentId)
        {
            return await _context.Nurses
                .Where(n => n.DepartamentId == departamentId && n.IsDeleted == false)
                .ToListAsync();
        }

        public async Task SetSpecialHoursBalance(Schedule schedule)
        {
            var nursesIds = schedule.ScheduleNurses.Select(n => n.NurseId);

            var nurses = await _context.Nurses.Where(n => nursesIds.Contains(n.NurseId)).ToListAsync();

            foreach(var nurse in nurses)
            {
                var scheduleNurse = schedule.ScheduleNurses.First(n => n.NurseId == nurse.NurseId);
                nurse.NightHoursBalance += scheduleNurse.NightHoursAssigned;
                nurse.HolidayHoursBalance += scheduleNurse.HolidayHoursAssigned;
            }

            NormaliseSpecialHoursBalannce(nurses);
        }

        private void NormaliseSpecialHoursBalannce(IEnumerable<Nurse> nurses)
        {
            var minNightHours = nurses.Min(n => n.NightHoursBalance);
            var minHolidayHours = nurses.Min(n => n.HolidayHoursBalance);

            foreach (var nurse in nurses)
            {
                nurse.NightHoursBalance -= minNightHours;
                nurse.HolidayHoursBalance -= minHolidayHours;
            }
        }
    }
}

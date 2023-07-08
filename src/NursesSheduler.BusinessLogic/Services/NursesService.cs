using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Services
{
    internal sealed class NursesService
    {
        private readonly IApplicationDbContext _context;

        public NursesService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Nurse>> GetDepartamentNurses(int departamentId)
        {
            return await _context.Nurses
                .Include(n => n.AbsencesSummaries)
                .Where(n => n.DepartamentId == departamentId && n.IsDeleted == false)
                .ToListAsync();
        }
    }
}

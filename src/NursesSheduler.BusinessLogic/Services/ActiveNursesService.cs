using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Services
{
    internal sealed class ActiveNursesService : IActiveNursesService
    {
        private readonly IApplicationDbContext _context;

        public ActiveNursesService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Nurse>> GetActiveDepartamentNurses(int departamentId)
        {
            return await _context.Nurses
                .Where(n => n.DepartamentId == departamentId && n.IsDeleted == false)
                .ToListAsync();
        }
    }
}

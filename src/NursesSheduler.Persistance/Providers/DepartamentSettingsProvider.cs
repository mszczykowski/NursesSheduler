using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure.Providers;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.Infrastructure.Providers
{
    public sealed class DepartamentSettingsProvider : CacheProvider<DepartamentSettings, int>, 
        IDepartamentSettingsProvider
    {
        private const string DEPT_SETTINGS_CACHE_KEY = "DeptSettings";
        private readonly IApplicationDbContext _context;

        public DepartamentSettingsProvider(IApplicationDbContext context, IMemoryCache memoryCache) 
            : base(memoryCache, DEPT_SETTINGS_CACHE_KEY)
        {
            _context = context;
        }

        protected override async Task<DepartamentSettings?> GetDataFromSource(int id)
        {
            return await _context.DepartamentSettings
                    .Include(s => s.Departament)
                    .FirstOrDefaultAsync(s => s.DepartamentId == id);
        }
    }
}

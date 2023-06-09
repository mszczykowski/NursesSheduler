using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NursesScheduler.BusinessLogic.Abstractions.CacheManagers;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Exceptions;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.CacheManagers
{
    internal class DepartamentSettingsManager : IDepartamentSettingsManager
    {
        private const string DEPT_SETTINGS_CACHE_KEY = "DeptSettings-";

        private readonly IApplicationDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public DepartamentSettingsManager(IApplicationDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public async Task<DepartamentSettings> GetDepartamentSettings(int departamentId)
        {
            DepartamentSettings result;
            var key = GetDeptSettingsCacheKey(departamentId);

            if (!_memoryCache.TryGetValue(key, out result))
            {
                result = await _context.DepartamentSettings.FirstOrDefaultAsync(s => s.DepartamentId == departamentId);

                if (result == null)
                    throw new EntityNotFoundException(departamentId, nameof(DepartamentSettings));

                _memoryCache.Set(key, result);
            }
            return result;
        }

        public void InvalidateCache(int departamentId)
        {
            var key = GetDeptSettingsCacheKey(departamentId);

            if (_memoryCache.TryGetValue(key, out var result))
            {
                _memoryCache.Remove(key);
            }
        }

        private string GetDeptSettingsCacheKey(int departamentId)
        {
            return $"{DEPT_SETTINGS_CACHE_KEY}{departamentId}";
        }
    }
}

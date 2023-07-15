using Microsoft.Extensions.Caching.Memory;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.Infrastructure.Providers
{
    public sealed class ScheduleStatsProvider : CacheProvider<ScheduleStats, ScheduleStatsKey>
    {
        private readonly IStatsService _statsService;
        public ScheduleStatsProvider(IStatsService statsService, IMemoryCache memoryCache, 
            string cacheKey) : base(memoryCache, cacheKey)
        {
            _statsService = statsService;
        }

        protected override async Task<ScheduleStats?> GetDataFromSource(ScheduleStatsKey key)
        {
            return await _statsService.GetScheduleStatsAsync(key.Year, key.Month, key.DepartamentId);
        }
    }
}

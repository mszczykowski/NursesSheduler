using Microsoft.Extensions.Caching.Memory;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure.Providers;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.Infrastructure.Providers
{
    public sealed class ScheduleStatsProvider : CacheProvider<ScheduleStats, ScheduleStatsKey>, IScheduleStatsProvider
    {
        private const string SCHED_STATS_CACHE_KEY = "ScheduleStats";

        private readonly IScheduleStatsService _statsService;

        public ScheduleStatsProvider(IScheduleStatsService statsService, IMemoryCache memoryCache) 
            : base(memoryCache, SCHED_STATS_CACHE_KEY)
        {
            _statsService = statsService;
        }

        protected override async Task<ScheduleStats?> GetDataFromSource(ScheduleStatsKey key)
        {
            return await _statsService.GetScheduleStatsAsync(key.Year, key.Month, key.DepartamentId);
        }
    }
}

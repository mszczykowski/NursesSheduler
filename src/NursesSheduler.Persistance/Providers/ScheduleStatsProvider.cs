using Microsoft.Extensions.Caching.Memory;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.Infrastructure.Providers
{
    public sealed class ScheduleStatsProvider : CacheProvider<ScheduleStats, ScheduleStatsKey>
    {
        private readonly IStatsService _scheduleStatsService;
        public ScheduleStatsProvider(IStatsService scheduleStatsService, IMemoryCache memoryCache, 
            string cacheKey) : base(memoryCache, cacheKey)
        {
            _scheduleStatsService = scheduleStatsService;
        }

        protected override async Task<ScheduleStats?> GetDataFromSource(ScheduleStatsKey id)
        {
            return await _scheduleStatsService.GetScheduleStatsAsync(id.Year, id.Month, id.DepartamentId);
        }
    }
}

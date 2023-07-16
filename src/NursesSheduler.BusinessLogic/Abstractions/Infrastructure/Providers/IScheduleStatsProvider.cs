using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.Abstractions.Infrastructure.Providers
{
    public interface IScheduleStatsProvider : ICacheProvider<ScheduleStats, ScheduleStatsKey>
    {

    }
}

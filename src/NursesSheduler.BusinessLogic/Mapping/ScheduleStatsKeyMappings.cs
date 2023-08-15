using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStats;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal sealed class ScheduleStatsKeyMappings : Profile
    {
        public ScheduleStatsKeyMappings()
        {
            CreateMap<GetScheduleStatsRequest, ScheduleStatsKey>();
        }
    }
}

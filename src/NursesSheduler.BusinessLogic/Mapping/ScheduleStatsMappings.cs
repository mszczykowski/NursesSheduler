using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.ScheduleStats.Queries.GetScheduleStatsQuery;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal class ScheduleStatsMappings : Profile
    {
        public ScheduleStatsMappings()
        {
            CreateMap<ScheduleStats, GetScheduleStatsResponse>();
        }
    }
}

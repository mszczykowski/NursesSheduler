using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.QuartersStats.Queries.GetQuarterStats;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStatsQuery;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal class ScheduleStatsMappings : Profile
    {
        public ScheduleStatsMappings()
        {
            CreateMap<ScheduleStats, GetScheduleStatsResponse>();

            CreateMap<GetQuarterStatsRequest.ScheduleStatsRequest, ScheduleStats>();
        }
    }
}

using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.QuartersStats.Queries.GetQuarterStats;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStats;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStatsFromSchedule;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal class ScheduleStatsMappings : Profile
    {
        public ScheduleStatsMappings()
        {
            CreateMap<ScheduleStats, GetScheduleStatsFromScheduleResponse>();

            CreateMap<ScheduleStats, GetScheduleStatsResponse>();

            CreateMap<GetQuarterStatsRequest.ScheduleStatsRequest, ScheduleStats>();
        }
    }
}

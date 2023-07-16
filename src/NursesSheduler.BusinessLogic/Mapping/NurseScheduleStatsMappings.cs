using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.QuartersStats.Queries.GetQuarterStats;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStatsQuery;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.RecalculateNursesScheduleStats;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal sealed class NurseScheduleStatsMappings : Profile
    {
        public NurseScheduleStatsMappings()
        {
            CreateMap<NurseScheduleStats, GetScheduleStatsResponse.NurseStatsResponse>();
            CreateMap<NurseScheduleStats, RecalculateNursesScheduleStatsResponse>();

            CreateMap<GetQuarterStatsRequest.NurseScheduleStatsRequest, NurseScheduleStats>();

            CreateMap<GetQuarterStatsResponse.NurseStatsResponse, NurseScheduleStats>();
        }
    }
}

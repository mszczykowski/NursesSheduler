using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.QuartersStats.Queries.GetQuarterStats;
using NursesScheduler.BusinessLogic.CommandsAndQueries.QuartersStats.Queries.RecalculateNurseQuarterStats;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStatsQuery;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.RecalculateNursesScheduleStats;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal sealed class NurseScheduleStatsMappings : Profile
    {
        public NurseScheduleStatsMappings()
        {
            CreateMap<NurseStats, GetScheduleStatsResponse.NurseStatsResponse>();
            CreateMap<NurseStats, RecalculateNursesScheduleStatsResponse>();

            CreateMap<GetQuarterStatsRequest.NurseStatsRequest, NurseStats>();

            CreateMap<GetQuarterStatsResponse.NurseStatsResponse, NurseStats>();

            CreateMap<RecalculateNurseQuarterStatsRequest.NurseStatsRequest, NurseStats>();
        }
    }
}

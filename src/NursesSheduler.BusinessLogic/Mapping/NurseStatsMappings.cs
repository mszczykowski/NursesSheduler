using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.QuartersStats.Queries.GetQuarterStats;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStats;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStatsFromSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.RecalculateNurseScheduleStats;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal sealed class NurseScheduleStatsMappings : Profile
    {
        public NurseScheduleStatsMappings()
        {
            CreateMap<NurseStats, GetScheduleStatsFromScheduleResponse.NurseStatsResponse>();

            CreateMap<NurseStats, GetScheduleStatsResponse.NurseStatsResponse>();

            CreateMap<NurseStats, RecalculateNurseStatsResponse.NursesStatsResponse>();

            CreateMap<GetQuarterStatsRequest.NurseStatsRequest, NurseScheduleStats>();
            CreateMap<NurseStats, GetQuarterStatsResponse.NurseStatsResponse>();
        }
    }
}

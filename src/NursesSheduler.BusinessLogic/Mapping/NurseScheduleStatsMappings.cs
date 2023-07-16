using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.ScheduleStats.Queries.GetScheduleStatsQuery;
using NursesScheduler.BusinessLogic.CommandsAndQueries.ScheduleStats.Queries.RecalculateNursesScheduleStats;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal sealed class NurseScheduleStatsMappings : Profile
    {
        public NurseScheduleStatsMappings()
        {
            CreateMap<NurseScheduleStats, GetScheduleStatsResponse.NurseScheduleStatsResponse>();
            CreateMap<NurseScheduleStats, RecalculateNursesScheduleStatsResponse>();
        }
    }
}

using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.QuartersStats.Queries.GetQuarterStats;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal class QuarterStatsMappings : Profile
    {
        public QuarterStatsMappings()
        {
            CreateMap<QuarterStats, GetQuarterStatsResponse>();
        }
    }
}

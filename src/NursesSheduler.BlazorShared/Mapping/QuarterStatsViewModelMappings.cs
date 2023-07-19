using AutoMapper;
using NursesScheduler.BlazorShared.Models.ViewModels;
using NursesScheduler.BusinessLogic.CommandsAndQueries.QuartersStats.Queries.GetQuarterStats;

namespace NursesScheduler.BlazorShared.Mapping
{
    internal sealed class QuarterStatsViewModelMappings : Profile
    {
        public QuarterStatsViewModelMappings()
        {
            CreateMap<GetQuarterStatsResponse, QuarterStatsViewModel>();
        }
    }
}

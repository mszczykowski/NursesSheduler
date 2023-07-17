using AutoMapper;
using NursesScheduler.BlazorShared.ViewModels;
using NursesScheduler.BusinessLogic.CommandsAndQueries.QuartersStats.Queries.GetQuarterStats;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStatsQuery;

namespace NursesScheduler.BlazorShared.Mappings
{
    internal class ScheduleStatsViewModelMappings : Profile
    {
        public ScheduleStatsViewModelMappings()
        {
            CreateMap<GetScheduleStatsResponse, ScheduleStatsViewModel>();

            CreateMap<ScheduleStatsViewModel, GetQuarterStatsRequest.ScheduleStatsRequest>();
        }
    }
}

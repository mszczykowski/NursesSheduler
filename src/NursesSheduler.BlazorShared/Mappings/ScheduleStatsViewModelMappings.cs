using AutoMapper;
using NursesScheduler.BlazorShared.Models.ViewModels;
using NursesScheduler.BusinessLogic.CommandsAndQueries.QuartersStats.Queries.GetQuarterStats;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStats;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStatsQuery;

namespace NursesScheduler.BlazorShared.Mappings
{
    internal sealed class ScheduleStatsViewModelMappings : Profile
    {
        public ScheduleStatsViewModelMappings()
        {
            CreateMap<GetScheduleStatsFromScheduleResponse, ScheduleStatsViewModel>();
            
            CreateMap<GetScheduleStatsResponse, ScheduleStatsViewModel>();

            CreateMap<ScheduleStatsViewModel, GetQuarterStatsRequest.ScheduleStatsRequest>();
        }
    }
}

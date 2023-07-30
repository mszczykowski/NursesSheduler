using AutoMapper;
using NursesScheduler.BlazorShared.Models.ViewModels.ValueObjects;
using NursesScheduler.BusinessLogic.CommandsAndQueries.QuartersStats.Queries.GetQuarterStats;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStats;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStatsFromSchedule;

namespace NursesScheduler.BlazorShared.Mapping
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

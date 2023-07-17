using AutoMapper;
using NursesScheduler.BlazorShared.ViewModels;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStatsQuery;

namespace NursesScheduler.BlazorShared.Mapping
{
    internal sealed class ScheduleViewModelMappings : Profile
    {
        public ScheduleViewModelMappings()
        {
            CreateMap<GetScheduleResponse, ScheduleViewModel>();

            CreateMap<ScheduleViewModel, GetScheduleStatsRequest.ScheduleRequest>();
        }
    }
}

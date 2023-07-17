using AutoMapper;
using NursesScheduler.BlazorShared.ViewModels;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStatsQuery;

namespace NursesScheduler.BlazorShared.Mappings
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

using AutoMapper;
using NursesScheduler.BlazorShared.ViewModels;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule;

namespace NursesScheduler.BlazorShared.Mapping
{
    internal sealed class DayViewModelMappings : Profile
    {
        public DayViewModelMappings()
        {
            CreateMap<GetScheduleResponse.DayResponse, DayViewModel>();
        }
    }
}

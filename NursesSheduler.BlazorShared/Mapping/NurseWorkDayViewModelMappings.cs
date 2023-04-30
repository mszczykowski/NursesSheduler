using AutoMapper;
using NursesScheduler.BlazorShared.ViewModels;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule;

namespace NursesScheduler.BlazorShared.Mapping
{
    internal sealed class NurseWorkDayViewModelMappings : Profile
    {
        public NurseWorkDayViewModelMappings()
        {
            CreateMap<GetScheduleResponse.NurseWorkDayResponse, NurseWorkDayViewModel>();
        }
    }
}

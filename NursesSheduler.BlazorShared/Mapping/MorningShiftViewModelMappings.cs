using AutoMapper;
using NursesScheduler.BlazorShared.ViewModels;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule;

namespace NursesScheduler.BlazorShared.Mapping
{
    internal sealed class MorningShiftViewModelMappings : Profile
    {
        public MorningShiftViewModelMappings()
        {
            CreateMap<GetScheduleResponse.MorningShiftsResponse, MorningShiftViewModel>();
        }
    }
}

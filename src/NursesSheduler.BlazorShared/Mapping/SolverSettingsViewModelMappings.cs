using AutoMapper;
using NursesScheduler.BlazorShared.Models.ViewModels;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.SolveSchedule;

namespace NursesScheduler.BlazorShared.Mapping
{
    internal sealed class SolverSettingsViewModelMappings : Profile
    {
        public SolverSettingsViewModelMappings()
        {
            CreateMap<SolverSettingsViewModel, SolveScheduleRequest.SolverSettingsRequest>();
            CreateMap<SolveScheduleResponse.SolverSettingsResponse, SolverSettingsViewModel>();
        }
    }
}

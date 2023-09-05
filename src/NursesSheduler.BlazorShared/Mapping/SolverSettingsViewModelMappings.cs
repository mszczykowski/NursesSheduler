using AutoMapper;
using NursesScheduler.BlazorShared.Models.ViewModels.Entities;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Commands.SolveSchedule;

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

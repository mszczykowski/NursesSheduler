using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Commands.SolveSchedule;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Mapping
{
    public sealed class SolverSettingsMappings : Profile
    {
        public SolverSettingsMappings()
        {
            CreateMap<SolveScheduleRequest.SolverSettingsRequest, SolverSettings>();

            CreateMap<SolverSettings, SolveScheduleResponse.SolverSettingsResponse>();
        }
    }
}

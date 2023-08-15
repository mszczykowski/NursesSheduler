using AutoMapper;
using NursesScheduler.BlazorShared.Models.ViewModels.ValueObjects;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.RecalculateNurseScheduleStats;

namespace NursesScheduler.BlazorShared.Mapping
{
    internal sealed class ScheduleValidationErrorViewModelMappings : Profile
    {
        public ScheduleValidationErrorViewModelMappings()
        {
            CreateMap<RecalculateNurseStatsResponse.ScheduleValidationErrorResponse, ScheduleValidationErrorViewModel>();
        }
    }
}

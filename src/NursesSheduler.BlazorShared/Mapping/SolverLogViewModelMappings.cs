using AutoMapper;
using NursesScheduler.BlazorShared.Models.ViewModels.ValueObjects;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SolverLogs.Queries.GetSolverLogs;

namespace NursesScheduler.BlazorShared.Mapping
{
    internal sealed class SolverLogViewModelMappings : Profile
    {
        public SolverLogViewModelMappings()
        {
            CreateMap<GetSolverLogsResponse, SolverLogViewModel>();
        }
    }
}

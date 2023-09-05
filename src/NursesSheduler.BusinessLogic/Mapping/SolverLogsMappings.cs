using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SolverLogs.Queries.GetSolverLogs;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal class SolverLogsMappings : Profile
    {
        public SolverLogsMappings()
        {
            CreateMap<SolverLog, GetSolverLogsResponse>();
        }
    }
}

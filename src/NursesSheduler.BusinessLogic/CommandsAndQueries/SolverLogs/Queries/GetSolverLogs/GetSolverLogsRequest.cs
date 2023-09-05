using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.SolverLogs.Queries.GetSolverLogs
{
    public sealed class GetSolverLogsRequest : IRequest<IEnumerable<GetSolverLogsResponse>>
    {
    }
}

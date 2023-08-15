using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.SolverLogs.Queries
{
    public sealed class GetSolverLogsRequest : IRequest<IEnumerable<GetSolverLogsResponse>>
    {
    }
}

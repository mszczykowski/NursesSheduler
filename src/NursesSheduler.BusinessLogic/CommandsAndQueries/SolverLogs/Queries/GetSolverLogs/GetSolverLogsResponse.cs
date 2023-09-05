using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.SolverLogs.Queries.GetSolverLogs
{
    public sealed class GetSolverLogsResponse
    {
        public DateTime LogDate { get; set; }
        public SolverEvents SolverEvent { get; set; }
        public SolverAbortReasons? AbortReason { get; set; }
        public int CurrentSolverRun { get; set; }
    }
}

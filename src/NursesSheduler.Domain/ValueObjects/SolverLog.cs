using NursesScheduler.Domain.Enums;

namespace NursesScheduler.Domain.ValueObjects
{
    public sealed class SolverLog
    {
        public DateTime LogDate { get; set; }
        public SolverEvents SolverEvent { get; set; }
        public SolverAbortReasons? AbortReason { get; set; }
        public int CurrentSolverRun { get; set; }
    }
}

namespace NursesScheduler.Domain.Enums
{
    public enum SolverEvents
    {
        Started,
        TimedOut,
        CanceledByUser,
        RetriesExhausted,
        SolutionNotFound,
        SolutionFound,
        Finished,
        Aborted,
    }
}

using NursesScheduler.Domain.Enums;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Abstractions.Services
{
    internal interface ISolverLoggerService
    {
        IReadOnlyCollection<SolverLog> SolverLogs { get; }

        void InitialiseLogger(Action subscriberMethod);
        void LogEvent(SolverEvents solverEvent);
        void LogEvent(SolverEvents solverEvent, SolverAbortReasons abortReason);
    }
}

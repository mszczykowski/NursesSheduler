using NursesScheduler.BusinessLogic.Abstractions.Solver.States;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Abstractions.Solver.Queue
{
    internal interface INursesQueue
    {
        bool IsEmpty();
        void PopulateQueue(ISolverState solverState, Day day);
        bool TryDequeue(out int result, bool isFirstTry);
    }
}
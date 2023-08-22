using NursesScheduler.BusinessLogic.Abstractions.Solver.States;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Abstractions.Solver.Directors
{
    internal interface INursesQueueDirector
    {
        Queue<int> BuildSortedNursesQueue(ISolverState solverState, Day day);
    }
}
using NursesScheduler.BusinessLogic.Abstractions.Solver.Managers;
using NursesScheduler.BusinessLogic.Abstractions.Solver.States;

namespace NursesScheduler.BusinessLogic.Abstractions.Solver
{
    internal interface IScheduleSolver
    {
        ISolverState? TrySolveSchedule(Random random);
    }
}

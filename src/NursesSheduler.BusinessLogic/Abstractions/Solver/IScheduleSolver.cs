using NursesScheduler.BusinessLogic.Abstractions.Solver.StateManagers;

namespace NursesScheduler.BusinessLogic.Abstractions.Solver
{
    internal interface IScheduleSolver
    {
        ISolverState GenerateSchedule(int numberOfRetries);
    }
}

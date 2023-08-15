using NursesScheduler.BusinessLogic.Abstractions.Solver.States;

namespace NursesScheduler.BusinessLogic.Abstractions.Solver.Constraints
{
    internal interface IConstraint
    {
        bool IsSatisfied(ISolverState currentSolverState, INurseState currentNurseState, TimeSpan shiftLengthToAssing);
    }
}

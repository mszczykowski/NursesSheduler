using NursesScheduler.BusinessLogic.Abstractions.Solver.Constraints;
using NursesScheduler.BusinessLogic.Abstractions.Solver.StateManagers;

namespace NursesScheduler.BusinessLogic.Solver.Constraints
{
    internal sealed class BreakConstraint : IConstraint
    {
        private readonly TimeSpan _minimalBreak;

        public BreakConstraint(TimeSpan minimalBreak)
        {
            _minimalBreak = minimalBreak;
        }

        public bool IsSatisfied(ISolverState currentSolverState, INurseState currentNurseState, TimeSpan shiftLengthToAssing)
        {
            if(currentNurseState.HoursFromLastShift <= _minimalBreak && currentNurseState.HoursToNextShift <= _minimalBreak)
                return true;

            return false;
        }
    }
}

using NursesScheduler.BusinessLogic.Abstractions.Solver.Constraints;
using NursesScheduler.BusinessLogic.Abstractions.Solver.StateManagers;

namespace NursesScheduler.BusinessLogic.Solver.Constraints
{
    internal sealed class MaxTotalHoursInWeekConstraint : IConstraint
    {
        private readonly TimeSpan _maxWorkTimeInMonth;
        public MaxTotalHoursInWeekConstraint(TimeSpan maxWorkTimeInMonth)
        {
            _maxWorkTimeInMonth = maxWorkTimeInMonth;
        }

        public bool IsSatisfied(ISolverState currentSolverState, INurseState currentNurseState, 
            TimeSpan shiftLengthToAssing)
        {
            return (currentNurseState
                .WorkTimeAssignedInWeek[currentSolverState.WeekInQuarter] 
                + shiftLengthToAssing) 
                < _maxWorkTimeInMonth;
        }
    }
}

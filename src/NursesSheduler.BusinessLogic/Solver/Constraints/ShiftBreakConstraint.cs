using NursesScheduler.BusinessLogic.Abstractions.Solver.Constraints;
using NursesScheduler.BusinessLogic.Abstractions.Solver.States;
using NursesScheduler.BusinessLogic.Solver.Enums;
using NursesScheduler.Domain.Constants;

namespace NursesScheduler.BusinessLogic.Solver.Constraints
{
    internal sealed class ShiftBreakConstraint : IConstraint
    {
        private readonly TimeSpan _minimalShiftBreak;

        private int _calculatedForDay;
        private ShiftIndex _calculatedForShift;
        private TimeSpan _timeToScheduleEnd;

        public ShiftBreakConstraint(TimeSpan minimalBreak)
        {
            _minimalShiftBreak = minimalBreak;
            _calculatedForDay = -1;
        }

        public bool IsSatisfied(ISolverState currentSolverState, INurseState currentNurseState, 
            TimeSpan shiftLengthToAssing)
        {
            if(currentNurseState.HoursFromLastShift >= _minimalShiftBreak)
            {
                return false;
            }

            if(currentSolverState.CurrentShift == ShiftIndex.Day)
            {
                return currentNurseState
                    .HoursToNextShiftMatrix[currentSolverState.CurrentDay - 1] 
                    - 
                    ScheduleConstatns.RegularShiftLenght
                    >=
                    _minimalShiftBreak;
            }
            else
            {
                return currentNurseState
                   .HoursToNextShiftMatrix[currentSolverState.CurrentDay]
                   -
                   ScheduleConstatns.RegularShiftLenght
                   >=
                   _minimalShiftBreak;
            }
        }
    }
}

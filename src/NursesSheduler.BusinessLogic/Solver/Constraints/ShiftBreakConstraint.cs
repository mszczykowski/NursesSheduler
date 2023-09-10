using NursesScheduler.BusinessLogic.Abstractions.Solver.Constraints;
using NursesScheduler.BusinessLogic.Abstractions.Solver.States;
using NursesScheduler.BusinessLogic.Solver.Enums;
using NursesScheduler.Domain.Constants;

namespace NursesScheduler.BusinessLogic.Solver.Constraints
{
    internal sealed class ShiftBreakConstraint : IConstraint
    {
        private readonly TimeSpan _minimalShiftBreak;

        public ShiftBreakConstraint(TimeSpan minimalBreak)
        {
            _minimalShiftBreak = minimalBreak;
        }

        public bool IsSatisfied(int currentDay, ShiftIndex currentShift, INurseState currentNurseState, 
            TimeSpan shiftLengthToAssing)
        {
            if (currentNurseState.HoursFromLastShift < _minimalShiftBreak)
            {
                return false;
            }

            if(currentShift == ShiftIndex.Day)
            {
                return currentNurseState
                    .HoursToNextShiftMatrix[currentDay - 1] 
                    - 
                    ScheduleConstatns.RegularShiftLength
                    >=
                    _minimalShiftBreak;
            }
            else
            {
                return currentNurseState
                   .HoursToNextShiftMatrix[currentDay]
                   -
                   ScheduleConstatns.RegularShiftLength
                   >=
                   _minimalShiftBreak;
            }
        }
    }
}

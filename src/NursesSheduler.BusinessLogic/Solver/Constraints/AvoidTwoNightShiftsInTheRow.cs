using NursesScheduler.BusinessLogic.Abstractions.Solver.Constraints;
using NursesScheduler.BusinessLogic.Abstractions.Solver.States;
using NursesScheduler.BusinessLogic.Solver.Enums;

namespace NursesScheduler.BusinessLogic.Solver.Constraints
{
    internal sealed class AvoidTwoNightShiftsInTheRow : IConstraint
    {
        public bool IsSatisfied(int currentDay, ShiftIndex currentShift, INurseState currentNurseState, TimeSpan shiftLengthToAssing)
        {
            if(currentDay > 1 && currentShift == ShiftIndex.Night &&
                currentNurseState.ScheduleRow[currentDay - 2] == Domain.Enums.ShiftTypes.Night)
            {
                return false;
            }

            return true;
        }
    }
}

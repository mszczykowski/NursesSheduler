using NursesScheduler.BusinessLogic.Abstractions.Solver.Constraints;
using NursesScheduler.BusinessLogic.Abstractions.Solver.States;
using NursesScheduler.BusinessLogic.Solver.Enums;

namespace NursesScheduler.BusinessLogic.Solver.Constraints
{
    internal sealed class HasEnoughWorkTimeLeft : IConstraint
    {
        public bool IsSatisfied(int currentDay, ShiftIndex currentShift, INurseState currentNurseState, TimeSpan shiftLengthToAssing)
        {
            return currentNurseState.WorkTimeInQuarterLeft - shiftLengthToAssing >= TimeSpan.Zero;
        }
    }
}

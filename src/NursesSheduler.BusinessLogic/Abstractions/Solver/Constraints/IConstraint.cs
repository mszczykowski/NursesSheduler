using NursesScheduler.BusinessLogic.Abstractions.Solver.States;
using NursesScheduler.BusinessLogic.Solver.Enums;

namespace NursesScheduler.BusinessLogic.Abstractions.Solver.Constraints
{
    internal interface IConstraint
    {
        bool IsSatisfied(int currentDay, ShiftIndex currentShift, INurseState currentNurseState, TimeSpan shiftLengthToAssing);
    }
}

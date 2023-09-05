using NursesScheduler.BusinessLogic.Abstractions.Solver.Constraints;
using NursesScheduler.BusinessLogic.Abstractions.Solver.States;

namespace NursesScheduler.BusinessLogic.Solver.Constraints
{
    internal sealed class AvoidTwoNightShiftsInTheRow : IConstraint
    {
        public bool IsSatisfied(ISolverState currentSolverState, INurseState currentNurseState, TimeSpan shiftLengthToAssing)
        {
            if(currentSolverState.CurrentDay > 1 && currentSolverState.CurrentShift == Enums.ShiftIndex.Night && 
                currentSolverState
                    .ScheduleState[currentNurseState.NurseId][currentSolverState.CurrentDay - 2] == Domain.Enums.ShiftTypes.Night)
            {
                return false;
            }

            return true;
        }
    }
}

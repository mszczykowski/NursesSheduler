﻿using NursesScheduler.BusinessLogic.Abstractions.Solver.Constraints;
using NursesScheduler.BusinessLogic.Abstractions.Solver.States;

namespace NursesScheduler.BusinessLogic.Solver.Constraints
{
    internal sealed class HasEnoughWorkTimeLeft : IConstraint
    {
        public bool IsSatisfied(ISolverState currentSolverState, INurseState currentNurseState, TimeSpan shiftLengthToAssing)
        {
            //if(currentSolverState.CurrentDay >= 2) return false;
            return currentNurseState.WorkTimeInQuarterLeft - shiftLengthToAssing >= TimeSpan.Zero;
        }
    }
}

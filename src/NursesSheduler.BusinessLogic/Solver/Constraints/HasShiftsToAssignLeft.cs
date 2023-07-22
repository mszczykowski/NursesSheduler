﻿using NursesScheduler.BusinessLogic.Abstractions.Solver.Constraints;
using NursesScheduler.BusinessLogic.Abstractions.Solver.States;

namespace NursesScheduler.BusinessLogic.Solver.Constraints
{
    internal sealed class HasShiftsToAssignLeft : IConstraint
    {
        public bool IsSatisfied(ISolverState currentSolverState, INurseState currentNurseState, TimeSpan shiftLengthToAssing)
        {
            return currentNurseState.NumberOfRegularShiftsToAssign > 0;
        }
    }
}

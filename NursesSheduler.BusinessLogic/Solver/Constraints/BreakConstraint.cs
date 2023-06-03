using NursesScheduler.BusinessLogic.Abstractions.Solver.Constraints;
using NursesScheduler.BusinessLogic.Abstractions.Solver.StateManagers;
using NursesScheduler.BusinessLogic.Solver.Enums;

namespace NursesScheduler.BusinessLogic.Solver.Constraints
{
    internal sealed class BreakConstraint : IConstraint
    {
        private readonly TimeSpan _minimalBreak;

        private int _calculatedForDay;
        private ShiftIndex _calculatedForShift;
        private TimeSpan _timeToScheduleEnd;

        public BreakConstraint(TimeSpan minimalBreak)
        {
            _minimalBreak = minimalBreak;
            _calculatedForDay = -1;
        }

        public bool IsSatisfied(ISolverState currentSolverState, INurseState currentNurseState, 
            TimeSpan shiftLengthToAssing)
        {
            SetTimeToScheuleEnd(currentSolverState);

            if (currentNurseState.HoursFromLastShift >= _minimalBreak && 
                (currentNurseState.HoursToNextShift >= _minimalBreak || 
                currentNurseState.HoursToNextShift >= _timeToScheduleEnd))
                return true;

            return false;
        }

        private void SetTimeToScheuleEnd(ISolverState currentSolverState)
        {
            if(currentSolverState.CurrentDay != _calculatedForDay || 
                currentSolverState.CurrentShift != _calculatedForShift)
            {
                _timeToScheduleEnd = currentSolverState.GetHoursToScheduleEnd();
            }
        }
    }
}

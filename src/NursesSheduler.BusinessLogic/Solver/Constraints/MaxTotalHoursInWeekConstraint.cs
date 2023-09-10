using NursesScheduler.BusinessLogic.Abstractions.Solver.Constraints;
using NursesScheduler.BusinessLogic.Abstractions.Solver.States;
using NursesScheduler.BusinessLogic.Solver.Enums;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Solver.Constraints
{
    internal sealed class MaxTotalHoursInWeekConstraint : IConstraint
    {
        private readonly TimeSpan _maxWorkTimeInMonth;
        private readonly DayNumbered[] _monthDays;
        public MaxTotalHoursInWeekConstraint(TimeSpan maxWorkTimeInMonth, IEnumerable<DayNumbered> days)
        {
            _maxWorkTimeInMonth = maxWorkTimeInMonth;
            _monthDays = days.OrderBy(d => d.Date.Day).ToArray();
        }

        public bool IsSatisfied(int currentDay, ShiftIndex currentShift, INurseState currentNurseState, 
            TimeSpan shiftLengthToAssing)
        {
            return (currentNurseState
                .WorkTimeAssignedInWeeks[_monthDays[currentDay - 1].WeekInQuarter - 1] 
                + shiftLengthToAssing) 
                <= _maxWorkTimeInMonth;
        }
    }
}

using ScheduleSolver.Interfaces.Constraints;
using ScheduleSolver.Interfaces.StateManagers;
using SheduleSolver.Domain.Models;
using SheduleSolver.Domain.Models.Calendar;

namespace ScheduleSolver.Implementation.Constraints
{
    internal sealed class MaxTotalHoursInWeekConstraint : IConstraint
    {
        public bool IsSatisfied(ISolverState currentState, IEmployeeState currentEmployee, Month month,
            WorkTimeConfiguration workTimeConfiguration, TimeSpan shiftLenght)
        {
            bool x = currentEmployee.WorkTimeAssignedInWeek[(int)Math.Ceiling((double)month.Days[currentState.CurrentDay - 1].DayInQuarter / 7) - 1]
                + shiftLenght < workTimeConfiguration.MaximumWorkTimeInWeek;

            if (!x)
            {
                Console.WriteLine($"Nurse {currentEmployee.Id} too much hours in week!");
                return false;
            }
            else return x;
        }
    }
}

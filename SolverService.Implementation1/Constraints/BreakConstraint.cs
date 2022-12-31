using SheduleSolver.Domain.Models;
using SheduleSolver.Domain.Models.Calendar;
using SolverService.Interfaces.Constraints;
using SolverService.Interfaces.StateManagers;

namespace SolverService.Implementation.Constraints
{
    internal sealed class BreakConstraint : IConstraint
    {
        public bool IsSatisfied(ISolverState currentState, IEmployeeState currentEmployee, Month month,
            WorkTimeConfiguration workTimeConfiguration, TimeSpan shiftLenght)
        {
            var previousShift = currentState.GetPreviousShift();
            if (previousShift != null && previousShift.Contains(currentEmployee.Id))
            {
                Console.WriteLine($"Nurse {currentEmployee.Id} break to short!");
                return false;
            }


            var nextShift = currentState.GetNextShift();
            if (nextShift != null && nextShift.Contains(currentEmployee.Id))
            {
                Console.WriteLine($"Nurse {currentEmployee.Id} break to short!");
                return false;
            }


            return true;
        }
    }
}

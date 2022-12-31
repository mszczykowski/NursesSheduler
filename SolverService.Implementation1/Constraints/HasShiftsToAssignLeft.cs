using SheduleSolver.Domain.Models;
using SheduleSolver.Domain.Models.Calendar;
using SolverService.Interfaces.Constraints;
using SolverService.Interfaces.StateManagers;

namespace SolverService.Implementation.Constraints
{
    internal sealed class HasShiftsToAssignLeft : IConstraint
    {
        public bool IsSatisfied(ISolverState currentState, IEmployeeState currentEmployee, Month month,
            WorkTimeConfiguration workTimeConfiguration, TimeSpan shiftLenght)
        {
            bool x = currentEmployee.NumberOfShiftsToAssign > 0;
            if (!x)
            {
                Console.WriteLine($"Nurse {currentEmployee.Id} no shifts left!");
                return false;
            }
            else return x;
        }
    }
}

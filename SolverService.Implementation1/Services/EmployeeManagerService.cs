using SheduleSolver.Domain.Enums;
using SheduleSolver.Implementation.Builders;
using SheduleSolver.Implementation.StateManagers;
using SolverService.Interfaces.Builders;
using SolverService.Interfaces.Services;
using SolverService.Interfaces.StateManagers;

namespace SolverService.Implementation.Services
{
    internal sealed class EmployeeManagerService : IEmployeeManagerService
    {
        private IEmployeeQueueBuilder queueBuilder;
        public Queue<int> GetSortedEmployeeQueue(ShiftType shitType, bool isHolidayShift,
            List<int> previousDayShift, List<IEmployeeState> employees, int currentDay, Random random)
        {
            queueBuilder = new EmployeeQueueBuilder(employees, random);

            switch (shitType, isHolidayShift)
            {
                case (ShiftType.day, false):
                    return GetEmployeesForDayShift(currentDay);
                case (ShiftType.day, true):
                    return GetEmployeesForDayHolidayShift(currentDay);
                case (ShiftType.night, false):
                    return GetEmployeesForNightShift(currentDay, previousDayShift);
                case (ShiftType.night, true):
                    return GetEmployeesForNightHolidayShift(currentDay, previousDayShift);
                default:
                    return queueBuilder.GetResult();
            }
        }

        private Queue<int> GetEmployeesForDayShift(int currentDay)
        {
            return queueBuilder.RemoveEmployeesOnPTO(currentDay).OrderByLongestBreak().GetResult();
        }

        private Queue<int> GetEmployeesForDayHolidayShift(int currentDay)
        {
            return queueBuilder.RemoveEmployeesOnPTO(currentDay).OrderByLowestNumberOfHolidayShitfs()
                .OrderByLongestBreak().GetResult();
        }

        private Queue<int> GetEmployeesForNightShift(int currentDay, List<int> previousDayShift)
        {
            return queueBuilder.ProritisePreviousDayShiftWorkers(previousDayShift).RemoveEmployeesOnPTO(currentDay)
                .OrderByLowestNumberOfNightShitfs().GetResult();
        }


        private Queue<int> GetEmployeesForNightHolidayShift(int currentDay, List<int> previousShift)
        {
            return queueBuilder.ProritisePreviousDayShiftWorkers(previousShift).RemoveEmployeesOnPTO(currentDay)
                .OrderByLowestNumberOfNightShitfs().GetResult();
        }
    }
}

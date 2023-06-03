using NursesScheduler.BusinessLogic.Abstractions.Solver.Builders;
using NursesScheduler.BusinessLogic.Abstractions.Solver.Directors;
using NursesScheduler.BusinessLogic.Abstractions.Solver.StateManagers;
using NursesScheduler.BusinessLogic.Solver.Builders;
using NursesScheduler.BusinessLogic.Solver.Enums;

namespace NursesScheduler.BusinessLogic.Solver.Directors
{
    internal sealed class NurseQueueDirector : INurseQueueDirector
    {
        private readonly Random _random;

        public NurseQueueDirector(Random random)
        {
            _random = random;
        }

        private INurseQueueBuilder _nurseQueueBuilder;

        public Queue<int> GetSortedEmployeeQueue(ShiftIndex shiftIndex, bool isWorkingDay,
            List<int> previousDayShift, ICollection<INurseState> nurses, int currentDay)
        {
            _nurseQueueBuilder = new NurseQueueBuilder(nurses, _random);

            switch (shiftIndex, isWorkingDay)
            {
                case (ShiftIndex.Day, true):
                    return GetEmployeesForDayShift(currentDay);
                case (ShiftIndex.Day, false):
                    return GetEmployeesForDayHolidayShift(currentDay);
                case (ShiftIndex.Night, true):
                    return GetEmployeesForNightShift(currentDay, previousDayShift);
                case (ShiftIndex.Night, false):
                    return GetEmployeesForNightHolidayShift(currentDay, previousDayShift);
                default:
                    return _nurseQueueBuilder.GetResult();
            }
        }

        private Queue<int> GetEmployeesForDayShift(int currentDay)
        {
            return _nurseQueueBuilder
                .RemoveEmployeesOnPTO(currentDay)
                .OrderByLongestBreak()
                .GetResult();
        }

        private Queue<int> GetEmployeesForDayHolidayShift(int currentDay)
        {
            return _nurseQueueBuilder
                .RemoveEmployeesOnPTO(currentDay)
                .OrderByLowestNumberOfHolidayShitfs()
                .OrderByLongestBreak()
                .GetResult();
        }

        private Queue<int> GetEmployeesForNightShift(int currentDay, List<int> previousDayShift)
        {
            return _nurseQueueBuilder
                .ProritisePreviousDayShiftWorkers(previousDayShift)
                .RemoveEmployeesOnPTO(currentDay)
                .OrderByLowestNumberOfNightShitfs()
                .GetResult();
        }


        private Queue<int> GetEmployeesForNightHolidayShift(int currentDay, List<int> previousShift)
        {
            return _nurseQueueBuilder
                .ProritisePreviousDayShiftWorkers(previousShift)
                .RemoveEmployeesOnPTO(currentDay)
                .OrderByLowestNumberOfNightShitfs()
                .GetResult();
        }
    }
}

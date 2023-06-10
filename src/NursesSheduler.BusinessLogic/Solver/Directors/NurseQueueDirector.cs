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
                    return GetNursesForDayShift(currentDay);
                case (ShiftIndex.Day, false):
                    return GetNursesForDayHolidayShift(currentDay);
                case (ShiftIndex.Night, true):
                    return GetNursesForNightShift(currentDay, previousDayShift);
                case (ShiftIndex.Night, false):
                    return GetNursesForNightHolidayShift(currentDay, previousDayShift);
                default:
                    return _nurseQueueBuilder.GetResult();
            }
        }

        private Queue<int> GetNursesForDayShift(int currentDay)
        {
            return _nurseQueueBuilder
                //.RemoveEmployeesOnPTO(currentDay)
                .OrderByLongestBreak()
                .GetResult();
        }

        private Queue<int> GetNursesForDayHolidayShift(int currentDay)
        {
            return _nurseQueueBuilder
                //.RemoveEmployeesOnPTO(currentDay)
                .OrderByLowestNumberOfHolidayShitfs()
                .OrderByLongestBreak()
                .GetResult();
        }

        private Queue<int> GetNursesForNightShift(int currentDay, List<int> previousDayShift)
        {
            return _nurseQueueBuilder
                .ProritisePreviousDayShiftWorkers(previousDayShift)
                //.RemoveEmployeesOnPTO(currentDay)
                .OrderByLowestNumberOfNightShitfs()
                .GetResult();
        }


        private Queue<int> GetNursesForNightHolidayShift(int currentDay, List<int> previousShift)
        {
            return _nurseQueueBuilder
                .ProritisePreviousDayShiftWorkers(previousShift)
                //.RemoveEmployeesOnPTO(currentDay)
                .OrderByLowestNumberOfNightShitfs()
                .GetResult();
        }
    }
}

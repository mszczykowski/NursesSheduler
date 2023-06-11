using NursesScheduler.BusinessLogic.Abstractions.Solver.Builders;
using NursesScheduler.BusinessLogic.Abstractions.Solver.Directors;
using NursesScheduler.BusinessLogic.Abstractions.Solver.StateManagers;
using NursesScheduler.BusinessLogic.Solver.Builders;
using NursesScheduler.BusinessLogic.Solver.Enums;

namespace NursesScheduler.BusinessLogic.Solver.Directors
{
    internal sealed class NurseQueueDirector : INurseQueueDirector
    {
        private INurseQueueBuilder _nurseQueueBuilder;

        public Queue<int> GetSortedEmployeeQueue(ShiftIndex shiftIndex, bool isWorkingDay,
            HashSet<int> previousDayShift, HashSet<INurseState> nurses, Random random)
        {
            _nurseQueueBuilder = new NurseQueueBuilder(nurses, random);

            switch (shiftIndex, isWorkingDay)
            {
                case (ShiftIndex.Day, true):
                    return GetNursesForDayShift();
                case (ShiftIndex.Day, false):
                    return GetNursesForDayHolidayShift();
                case (ShiftIndex.Night, true):
                    return GetNursesForNightShift(previousDayShift);
                case (ShiftIndex.Night, false):
                    return GetNursesForNightHolidayShift(previousDayShift);
                default:
                    return _nurseQueueBuilder.GetResult();
            }
        }

        private Queue<int> GetNursesForDayShift()
        {
            return _nurseQueueBuilder
                .OrderByLongestBreak()
                .GetResult();
        }

        private Queue<int> GetNursesForDayHolidayShift()
        {
            return _nurseQueueBuilder
                .OrderByLowestNumberOfHolidayShitfs()
                .OrderByLongestBreak()
                .GetResult();
        }

        private Queue<int> GetNursesForNightShift(HashSet<int> previousDayShift)
        {
            return _nurseQueueBuilder
                .ProritisePreviousDayShiftWorkers(previousDayShift)
                .OrderByLowestNumberOfNightShitfs()
                .GetResult();
        }


        private Queue<int> GetNursesForNightHolidayShift(HashSet<int> previousShift)
        {
            return _nurseQueueBuilder
                .ProritisePreviousDayShiftWorkers(previousShift)
                .OrderByLowestNumberOfNightShitfs()
                .GetResult();
        }
    }
}

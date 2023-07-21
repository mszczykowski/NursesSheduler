using NursesScheduler.BusinessLogic.Abstractions.Solver.Builders;
using NursesScheduler.BusinessLogic.Abstractions.Solver.Directors;
using NursesScheduler.BusinessLogic.Abstractions.Solver.StateManagers;
using NursesScheduler.BusinessLogic.Solver.Builders;
using NursesScheduler.BusinessLogic.Solver.Enums;
using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.Solver.Directors
{
    internal sealed class NurseQueueDirector : INurseQueueDirector
    {
        private INurseQueueBuilder _nurseQueueBuilder;

        public Queue<int> GetSortedEmployeeQueue(ShiftIndex shiftIndex, bool isWorkingDay,
            HashSet<int> previousDayShift, IEnumerable<INurseState> nurses, Random random)
        {
            _nurseQueueBuilder = new NurseQueueBuilder(nurses, random);

            switch (shiftIndex, isWorkingDay)
            {
                case (ShiftIndex.Day, true):
                    BuildQueueForDayShift();
                    break;
                case (ShiftIndex.Day, false):
                    BuildQueueForDayHolidayShift();
                    break;
                case (ShiftIndex.Night, true):
                    BuildQueueForNightShift(previousDayShift);
                    break;
                case (ShiftIndex.Night, false):
                    BuildQueueForNightHolidayShift(previousDayShift);
                    break;
            }

            return _nurseQueueBuilder.GetResult();
        }

        private void BuildQueueForDayShift()
        {
             _nurseQueueBuilder
                .OrderByLongestBreak();
        }

        private void BuildQueueForDayHolidayShift()
        {
            _nurseQueueBuilder
                .OrderByLowestNumberOfHolidayShitfs()
                .OrderByLongestBreak();
        }

        private void BuildQueueForNightShift(HashSet<int> previousDayShift)
        {
            _nurseQueueBuilder
                .ProritisePreviousDayShiftWorkers(previousDayShift)
                .OrderByLowestNumberOfNightShitfs();
        }


        private void BuildQueueForNightHolidayShift(HashSet<int> previousShift)
        {
            _nurseQueueBuilder
                .ProritisePreviousDayShiftWorkers(previousShift)
                .OrderByLowestNumberOfNightShitfs();
        }
    }
}

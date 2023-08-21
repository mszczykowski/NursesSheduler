using NursesScheduler.BusinessLogic.Abstractions.Solver.Builders;
using NursesScheduler.BusinessLogic.Abstractions.Solver.Directors;
using NursesScheduler.BusinessLogic.Abstractions.Solver.States;
using NursesScheduler.BusinessLogic.Solver.Builders;
using NursesScheduler.BusinessLogic.Solver.Enums;

namespace NursesScheduler.BusinessLogic.Solver.Directors
{
    internal sealed class NursesQueueDirector : INursesQueueDirector
    {
        private INurseQueueBuilder _nurseQueueBuilder;
        
        private readonly Random _random;

        public NursesQueueDirector(Random random)
        {
            _random = random;
        }

        public Queue<int> BuildSortedNursesQueue(ShiftIndex shiftIndex, bool IsWorkDay,
            HashSet<int> previousDayShift, IEnumerable<INurseState> nurses, int currentDay)
        {
            _nurseQueueBuilder = new NursesQueueBuilder(nurses, _random);

            _nurseQueueBuilder.ProritiseWorkersOnTimeOff(currentDay);

            switch (shiftIndex, IsWorkDay)
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

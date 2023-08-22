using NursesScheduler.BusinessLogic.Abstractions.Solver.Builders;
using NursesScheduler.BusinessLogic.Abstractions.Solver.Directors;
using NursesScheduler.BusinessLogic.Abstractions.Solver.States;
using NursesScheduler.BusinessLogic.Solver.Builders;
using NursesScheduler.BusinessLogic.Solver.Enums;
using NursesScheduler.Domain.Enums;
using NursesScheduler.Domain.ValueObjects;

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

        public Queue<int> BuildSortedNursesQueue(ISolverState solverState, Day day)
        {
            _nurseQueueBuilder = new NursesQueueBuilder(solverState.NurseStates
                .Where(n => solverState.ScheduleState[n.NurseId][solverState.CurrentDay - 1] == ShiftTypes.None), _random);

            switch (solverState.CurrentShift, day.IsWorkDay)
            {
                case (ShiftIndex.Day, true):
                    BuildQueueForDayShift(solverState.CurrentDay);
                    break;
                case (ShiftIndex.Day, false):
                    BuildQueueForDayHolidayShift(solverState.CurrentDay);
                    break;
                case (ShiftIndex.Night, true):
                    BuildQueueForNightShift(solverState.GetPreviousDayDayShift());
                    break;
                case (ShiftIndex.Night, false):
                    BuildQueueForNightHolidayShift(solverState.GetPreviousDayDayShift());
                    break;
            }

            return _nurseQueueBuilder.GetResult();
        }

        private void BuildQueueForDayShift(int currentDay)
        {
             _nurseQueueBuilder
                .ProritiseWorkersOnTimeOff(currentDay)
                .OrderByLongestBreak();
        }

        private void BuildQueueForDayHolidayShift(int currentDay)
        {
            _nurseQueueBuilder
                .ProritiseWorkersOnTimeOff(currentDay)
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

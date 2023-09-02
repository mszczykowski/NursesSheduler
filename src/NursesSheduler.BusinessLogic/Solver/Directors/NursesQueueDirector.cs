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
        private readonly Random _random;

        private IEnumerable<INurseState> _nurses;

        public NursesQueueDirector(Random random)
        {
            _random = random;
        }

        public Queue<int> BuildSortedNursesQueue(ISolverState solverState, Day day)
        {
            _nurses = solverState.NurseStates
                .Where(n => solverState.ScheduleState[n.NurseId][solverState.CurrentDay - 1] == ShiftTypes.None)
                .OrderBy(n => _random.Next());

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
                    BuildQueueForNightShift(solverState.GetPreviousDayDayShift());
                    break;
            }

            return new Queue<int>(_nurses.Select(n => n.NurseId));
        }

        private void BuildQueueForDayShift(int currentDay)
        {
            _nurses = _nurses
                .OrderByDescending(n => n.TimeOff[currentDay - 1])
                .ThenByDescending(n => n.HoursFromLastShift);
        }

        private void BuildQueueForDayHolidayShift(int currentDay)
        {
            _nurses = _nurses
                .OrderByDescending(n => n.TimeOff[currentDay - 1])
                .ThenBy(n => n.HolidayHoursAssigned)
                .ThenByDescending(n => n.HoursFromLastShift);
        }

        private void BuildQueueForNightShift(HashSet<int> previousDayShift)
        {
            _nurses = _nurses
                .OrderByDescending(n => previousDayShift.Contains(n.NurseId))
                .ThenBy(n => n.NightHoursAssigned);
        }
    }
}

using NursesScheduler.BusinessLogic.Abstractions.Solver.Directors;
using NursesScheduler.BusinessLogic.Abstractions.Solver.States;
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
                .Where(n => n.ScheduleRow[solverState.CurrentDay - 1] == ShiftTypes.None)
                .OrderBy(n => _random.Next());

            switch (solverState.CurrentShift, day.IsWorkDay)
            {
                case (ShiftIndex.Day, true):
                    BuildQueueForDayShift();
                    break;
                case (ShiftIndex.Day, false):
                    BuildQueueForDayHolidayShift();
                    break;
                case (ShiftIndex.Night, true):
                    BuildQueueForNightShift(solverState.GetPreviousDayDayShift(), solverState.CurrentDay);
                    break;
                case (ShiftIndex.Night, false):
                    BuildQueueForNightShift(solverState.GetPreviousDayDayShift(), solverState.CurrentDay);
                    break;
            }

            return new Queue<int>(_nurses.Select(n => n.NurseId));
        }

        private void BuildQueueForDayShift()
        {
            _nurses = _nurses
                .OrderByDescending(n => n.HoursFromLastShift)
                .ThenBy(n => n.NumberOfRegularShiftsToAssign);
        }

        private void BuildQueueForDayHolidayShift()
        {
            _nurses = _nurses
                .OrderBy(n => n.HolidayHoursAssigned)
                .OrderByDescending(n => n.HoursFromLastShift);
        }

        private void BuildQueueForNightShift(IEnumerable<int> previousDayShift, int currentDay)
        {
            _nurses = _nurses
                .OrderByDescending(n => previousDayShift.Contains(n.NurseId))
                .ThenBy(n => n.NightHoursAssigned);
        }
    }
}

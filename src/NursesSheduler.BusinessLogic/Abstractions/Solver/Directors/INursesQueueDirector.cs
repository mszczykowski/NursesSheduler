using NursesScheduler.BusinessLogic.Abstractions.Solver.States;
using NursesScheduler.BusinessLogic.Solver.Enums;

namespace NursesScheduler.BusinessLogic.Abstractions.Solver.Directors
{
    internal interface INursesQueueDirector
    {
        Queue<int> BuildSortedNursesQueue(ShiftIndex shiftIndex, bool IsWorkDay,
            HashSet<int> previousDayShift, IEnumerable<INurseState> nurses);
    }
}
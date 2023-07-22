using NursesScheduler.BusinessLogic.Abstractions.Solver.States;
using NursesScheduler.BusinessLogic.Solver.Enums;

namespace NursesScheduler.BusinessLogic.Abstractions.Solver.Directors
{
    internal interface INurseQueueDirector
    {
        Queue<int> GetSortedEmployeeQueue(ShiftIndex shiftIndex, bool IsWorkDay, HashSet<int> previousDayShift,
            IEnumerable<INurseState> nurses, Random random);
    }
}
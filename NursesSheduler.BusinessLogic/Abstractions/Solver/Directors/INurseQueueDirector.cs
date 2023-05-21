using NursesScheduler.BusinessLogic.Abstractions.Solver.StateManagers;
using NursesScheduler.BusinessLogic.Solver.Enums;

namespace NursesScheduler.BusinessLogic.Abstractions.Solver.Directors
{
    internal interface INurseQueueDirector
    {
        Queue<int> GetSortedEmployeeQueue(ShiftIndex shiftIndex, bool isHolidayShift, List<int> previousDayShift, 
            ICollection<INurseState> nurses, int currentDay);
    }
}
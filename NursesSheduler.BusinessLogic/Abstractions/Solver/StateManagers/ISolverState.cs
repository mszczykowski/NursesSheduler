using NursesScheduler.BusinessLogic.Solver.Enums;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Abstractions.Solver.StateManagers
{
    internal interface ISolverState
    {
        int CurrentDay { get; set; }
        int CurrentDayIndex { get; }
        ShiftIndex CurrentShift { get; set; }
        int DayNumberInQuarter { get; set; }
        int EmployeesToAssignForCurrentShift { get; set; }
        List<int>[] MorningShiftsState { get; }
        List<INurseState> Nurses { get; }
        List<int>[,] ScheduleState { get; }
        int ShortShiftsToAssign { get; set; }
        int WeekInQuarter { get; }

        void AdvanceState();
        void AssignEmployeeToMorningShift(INurseState nurse, MorningShift morningShift);
        void AssignNurseToRegularShift(INurseState nurse, bool isHoliday, DepartamentSettings departamentSettings);
        TimeSpan GetHoursToScheduleEnd();
        List<int> GetNextShift();
        List<int> GetPreviousDayShift();
        List<int> GetPreviousShift();
    }
}
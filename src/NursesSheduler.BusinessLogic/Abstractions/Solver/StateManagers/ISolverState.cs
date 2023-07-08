using NursesScheduler.BusinessLogic.Solver.Enums;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Abstractions.Solver.StateManagers
{
    internal interface ISolverState
    {
        int CurrentDay { get; set; }
        ShiftIndex CurrentShift { get; set; }
        int DayNumberInQuarter { get; set; }
        int NursesToAssignForCurrentShift { get; set; }
        HashSet<int>[] MorningShiftsState { get; }
        HashSet<INurseState> Nurses { get; }
        HashSet<int>[,] ScheduleState { get; }
        int NursesToAssignForMorningShift { get; set; }
        int WeekInQuarter { get; }
        int NursesToAssignOnTimeOff { get; set; }
        bool IsShiftAssined { get; }

        void AdvanceStateRegularShift();
        void AdvanceStateMorningShift();
        void AdvanceStateTimeOffShift();
        void AssignEmployeeToMorningShift(INurseState nurse, MorningShift morningShift);
        void AssignNurseToRegularShift(INurseState nurse, bool isHoliday, DepartamentSettings departamentSettings);
        void AssignNurseOnTimeOff(INurseState nurse, bool isHoliday, DepartamentSettings departamentSettings);
        TimeSpan GetHoursToScheduleEnd();
        HashSet<int> GetPreviousDayShift();
        void PopulateScheduleFromState(Schedule schedule);
    }
}
using NursesScheduler.BusinessLogic.Solver.Enums;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Abstractions.Solver.StateManagers
{
    internal interface INurseState
    {
        List<int> AssignedMorningShiftsIds { get; set; }
        TimeSpan HolidayPaidHoursAssigned { get; set; }
        TimeSpan HoursFromLastShift { get; set; }
        TimeSpan HoursToNextShift { get; set; }
        int NumberOfNightShiftsAssigned { get; set; }
        int NumberOfRegularShiftsToAssign { get; set; }
        int NumberOfTimeOffShiftsToAssign { get; set; }
        int NurseId { get; init; }
        //TimeSpan PTOTimeToAssign { get; set; }
        bool[] TimeOff { get; }
        TimeSpan[] WorkTimeAssignedInWeeks { get; set; }
        //TimeSpan WorkTimeToAssign { get; set; }
        //TimeSpan WorkTimeToAssignInQuarter { get; set; }
        bool HadMorningShiftAssigned { get; }
        PreviousNurseStates PreviousMonthLastShift { get; set; }

        void AdvanceState(TimeSpan hoursToNextShift);
        void UpdateStateOnMorningShiftAssign(MorningShift morningShift, int weekInQuarter, 
            TimeSpan hoursToNextShift);
        void UpdateStateOnRegularShiftAssign(bool isHoliday, ShiftIndex shiftIndex, int weekInQuarter, 
            DepartamentSettings departamentSettings, TimeSpan hoursToNextShift);
        void UpdateStateOnTimeOffShiftAssign(bool isHoliday, ShiftIndex shiftIndex, int weekInQuarter, 
            DepartamentSettings departamentSettings, TimeSpan hoursToNextShift);
        void ResetHoursFromLastShift();
    }
}
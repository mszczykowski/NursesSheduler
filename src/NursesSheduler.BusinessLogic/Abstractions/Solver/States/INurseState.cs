using NursesScheduler.BusinessLogic.Solver.Enums;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.Abstractions.Solver.States
{
    internal interface INurseState
    {
        int NurseId { get; }
        Dictionary<int, TimeSpan> WorkTimeAssignedInWeeks { get; }
        TimeSpan HoursFromLastShift { get; }
        TimeSpan[] HoursToNextShift { get; }
        int NumberOfRegularShiftsToAssign { get; }
        int NumberOfTimeOffShiftsToAssign { get; }
        TimeSpan HolidayPaidHoursAssigned { get; }
        TimeSpan NightHoursAssigned { get; }
        TimeSpan WorkTimeInQuarterLeft { get; }
        bool[] TimeOff { get; }
        HashSet<MorningShiftIndex> PreviouslyAssignedMorningShifts { get; }
        MorningShiftIndex? AssignedMorningShift { get; }
        ShiftTypes PreviousMonthLastShift { get; }
        NurseTeams NurseTeam { get; }


        IDictionary<int, MorningShiftIndex> AssignedMorningShifts { get; set; }



        void AdvanceState(TimeSpan hoursToNextShift);
        void UpdateStateOnMorningShiftAssign(MorningShift morningShift, int weekInQuarter,
            TimeSpan hoursToNextShift);
        void UpdateStateOnShiftAssign(bool isHoliday, ShiftIndex shiftIndex, int weekInQuarter,
            DepartamentSettings departamentSettings, TimeSpan hoursToNextShift);
        void UpdateStateOnTimeOffShiftAssign(bool isHoliday, ShiftIndex shiftIndex, int weekInQuarter,
            DepartamentSettings departamentSettings, TimeSpan hoursToNextShift);
        void ResetHoursFromLastShift();
    }
}
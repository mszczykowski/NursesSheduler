using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.BusinessLogic.Solver.Enums;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Enums;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Abstractions.Solver.States
{
    internal interface INurseState
    {
        MorningShiftIndex? AssignedMorningShift { get; }
        TimeSpan HolidayHoursAssigned { get; }
        TimeSpan HoursFromLastShift { get; }
        TimeSpan[] HoursToNextShiftMatrix { get; init; }
        TimeSpan NightHoursAssigned { get; }
        int NumberOfRegularShiftsToAssign { get; }
        int NumberOfTimeOffShiftsToAssign { get; }
        int NurseId { get; init; }
        NurseTeams NurseTeam { get; init; }
        HashSet<MorningShiftIndex> PreviouslyAssignedMorningShifts { get; init; }
        ShiftTypes PreviousMonthLastShift { get; init; }
        bool[] TimeOff { get; init; }
        Dictionary<int, TimeSpan> WorkTimeAssignedInWeeks { get; init; }
        TimeSpan WorkTimeInQuarterLeft { get; }

        void AdvanceState();
        void UpdateStateOnMorningShiftAssign(MorningShift morningShift, DayNumbered day, 
            DepartamentSettings departamentSettings, IWorkTimeService workTimeService);
        void UpdateStateOnRegularShiftAssign(ShiftIndex shiftIndex, DayNumbered day, 
            DepartamentSettings departamentSettings, IWorkTimeService workTimeService);
        void UpdateStateOnShiftAssign(ShiftTypes shiftType, TimeSpan shiftLenght, DayNumbered day, 
            DepartamentSettings departamentSettings, IWorkTimeService workTimeService);
        void UpdateStateOnTimeOffShiftAssign(ShiftIndex shiftIndex, DayNumbered day, 
            DepartamentSettings departamentSettings, IWorkTimeService workTimeService);
    }
}
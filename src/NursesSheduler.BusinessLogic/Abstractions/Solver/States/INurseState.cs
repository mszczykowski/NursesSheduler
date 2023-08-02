using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.BusinessLogic.Solver.Enums;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Enums;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Abstractions.Solver.States
{
    internal interface INurseState
    {
        TimeSpan HolidayHoursAssigned { get; }
        TimeSpan HoursFromLastShift { get; }
        TimeSpan[] HoursToNextShiftMatrix { get; }
        TimeSpan NightHoursAssigned { get; }
        int NumberOfRegularShiftsToAssign { get; }
        int NumberOfTimeOffShiftsToAssign { get; }
        int NurseId { get; }
        NurseTeams NurseTeam { get; }
        public HashSet<int> PreviouslyAssignedMorningShifts { get; init; }
        public int? AssignedMorningShiftId { get; set; }
        ShiftTypes PreviousMonthLastShift { get; }
        bool[] TimeOff { get; }
        Dictionary<int, TimeSpan> WorkTimeAssignedInWeeks { get; }
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
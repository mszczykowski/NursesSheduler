using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.BusinessLogic.Solver.Enums;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Enums;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Abstractions.Solver.States
{
    internal interface INurseState
    {
        int? AssignedMorningShiftId { get; set; }
        bool HadNumberOfShiftsReduced { get; set; }
        TimeSpan HolidayHoursAssigned { get; set; }
        TimeSpan HoursFromLastShift { get; }
        TimeSpan HoursToNextShift { get; }
        TimeSpan NextMonthHoursToNextShift { get; set; }
        TimeSpan NightHoursAssigned { get; set; }
        int NumberOfRegularShiftsToAssign { get; set; }
        int NumberOfTimeOffShiftsToAssign { get; set; }
        int NurseId { get; init; }
        NurseTeams NurseTeam { get; init; }
        HashSet<int> PreviouslyAssignedMorningShifts { get; init; }
        TimeSpan PreviousMonthHoursFromLastShift { get; set; }
        ShiftTypes PreviousMonthLastShift { get; init; }
        ShiftTypes[] ScheduleRow { get; set; }
        bool ShouldNurseSwapRegularForMorning { get; }
        bool[] TimeOff { get; init; }
        TimeSpan[] WorkTimeAssignedInWeeks { get; set; }
        TimeSpan WorkTimeInQuarterLeft { get; set; }

        void RecalculateHoursFromLastShift(int day);
        void RecalculateHoursToNextShift(int day);
        void UpdateStateOnMorningShiftAssign(MorningShift morningShift, DayNumbered day,
            DepartamentSettings departamentSettings, IWorkTimeService workTimeService);
        void UpdateStateOnRegularShiftAssign(ShiftIndex shiftIndex, DayNumbered day, 
            DepartamentSettings departamentSettings, IWorkTimeService workTimeService);
        void UpdateStateOnTimeOffShiftAssign(ShiftIndex shiftIndex, DayNumbered day, 
            DepartamentSettings departamentSettings, IWorkTimeService workTimeService);
        void RecalculateFromPreviousAndToNextShift(int day);
    }
}
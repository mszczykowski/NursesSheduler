using NursesScheduler.BusinessLogic.Solver.Enums;
using NursesScheduler.Domain.Models.Settings;

namespace NursesScheduler.BusinessLogic.Abstractions.Solver.StateManagers
{
    internal interface INurseState
    {
        int NurseId { get; }
        TimeSpan HoursFromLastShift { get; set; }
        TimeSpan HoursToNextShift { get; set; }
        int NumberOfNightShifts { get; set; }
        int NumberOfShiftsToAssign { get; set; }
        TimeSpan HolidayPaidHoursAssigned { get; set; }
        bool[] TimeOff { get; }
        TimeSpan PTOTimeToAssign { get; set; }
        TimeSpan WorkTimeToAssign { get; set; }
        TimeSpan WorkTimeToAssignInQuarter { get; set; }
        TimeSpan[] WorkTimeAssignedInWeek { get; set; }
        List<TimeSpan> AssignedMorningShifts { get; set; }
        void UpdateStateOnAssign(TimeSpan shiftLength, int weekInQuarter);
        void UpdateStateOnAssign(bool isHoliday, ShiftIndex shiftIndex, WorkTimeConfiguration workTimeConfiguration,
            int weekInQuarter);
        void AdvanceDaysFromLastShift();
    }
}
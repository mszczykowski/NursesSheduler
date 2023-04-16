using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule
{
    public sealed class GetScheduleResponse
    {
        public int ScheduleId { get; set; }
        public int DepartamentId { get; set; }
        public int MonthNumber { get; set; }
        public int Year { get; set; }
        public int Quarter { get; set; }
        public TimeSpan WorkTimeInMonth { get; set; }
        public TimeSpan PTOTimeAvailableToAssgin { get; set; }
        public TimeSpan PTOTimeAssigned { get; set; }
        public DayResponse[] Days { get; set; }
        public MorningShiftsResponse[] MorningShifts { get; set; }
        public bool ReadOnly { get; set; }
        public List<ScheduleRowResponse> ScheduleRows { get; set; }

        public sealed class DayResponse
        {
            public int DayNumber { get; set; }
            public bool IsHoliday { get; set; }
        }

        public sealed class ScheduleRowResponse
        {
            public int NurseId { get; init; }
            public PreviousNurseStates PreviousState { get; set; }
            public int DaysFromLastShift { get; set; }

            public ScheduleShiftResponse[] Shifts { get; }
            public TimeSpan PreviousMonthTime { get; init; }
            public TimeSpan AssignedTime { get; set; }
            public TimeSpan TimeToAssingInQuarterLeft { get; set; }
            public TimeSpan PTOTimeToAssign { get; set; }
        }

        public sealed class ScheduleShiftResponse
        {
            public ShiftTypes ShiftType { get; set; }
            public TimeOnly ShiftStart { get; set; }
            public TimeSpan ShiftEnd { get; set; }
            public bool IsTimeOff { get; set; }
            public AbsenceTypes TimeOffType { get; set; }
        }

        public sealed class MorningShiftsResponse
        {
            public int MonningShiftId { get; set; }
            public TimeSpan[] MorningShiftsLenght { get; set; }
        }
    }
}

using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule
{
    public sealed class GetScheduleResponse
    {
        public int ScheduleId { get; set; }
        public int DepartamentId { get; set; }
        public int MonthNumber { get; set; }
        public int Year { get; set; }
        public int QuarterId { get; set; }
        public TimeSpan WorkTimeInMonth { get; set; }
        public TimeSpan WorkTimeInQuarter { get; set; }
        public TimeSpan TimeOffAvailableToAssgin { get; set; }
        public TimeSpan TimeOffAssigned { get; set; }
        public bool IsClosed { get; set; }
        public ICollection<ScheduleNurseResponse> ScheduleNurses { get; set; }

        public sealed class ScheduleNurseResponse
        {
            public int ScheduleNurseId { get; set; }
            public int NurseId { get; init; }
            public PreviousNurseStates PreviousState { get; set; }
            public int DaysFromLastShift { get; set; }
            public NurseWorkDayResponse[] NurseWorkDays { get; set; }
            public TimeSpan PreviousMonthTime { get; set; }
            public TimeSpan TimeToAssingInMonth { get; set; }
            public TimeSpan TimeToAssingInQuarterLeft { get; set; }
            public TimeSpan TimeOffToAssign { get; set; }
            public TimeSpan HolidaysHoursAssigned { get; set; }
            public int NightShiftsAssigned { get; set; }
        }

        public sealed class NurseWorkDayResponse
        {
            public int NurseWorkDayId { get; set; }
            public ShiftTypes ShiftType { get; set; }
            public int DayNumber { get; set; }
            public bool IsTimeOff { get; set; }
            public MorningShiftIndex MorningShiftId { get; set; }
        }
    }
}

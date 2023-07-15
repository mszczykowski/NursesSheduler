using MediatR;
using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.RecalculateScheduleHours
{
    public sealed class RecalculateScheduleHoursRequest : IRequest<RecalculateScheduleHoursResponse>
    {
        public TimeSpan WorkTimeInQuarter { get; set; }
        public TimeSpan WorkTimeInMonth { get; set; }
        public TimeSpan TimeOffAvailableToAssgin { get; set; }
        public TimeSpan TimeOffAssigned { get; set; }
        public ScheduleNurseRequest ScheduleNurse { get; set; }
        public IEnumerable<MorningShiftRequest> MorningShifts { get; set; }
        public IEnumerable<DayRequest> Days { get; set; }

        public sealed class ScheduleNurseRequest
        {
            public int ScheduleNurseId { get; set; }
            public int NurseId { get; init; }
            public PreviousNurseStates PreviousState { get; set; }
            public int DaysFromLastShift { get; set; }
            public NurseWorkDayRequest[] NurseWorkDays { get; set; }
            public TimeSpan PreviousMonthTime { get; set; }
            public TimeSpan TimeToAssingInMonth { get; set; }
            public TimeSpan TimeToAssingInQuarterLeft { get; set; }
            public TimeSpan TimeOffToAssign { get; set; }
            public TimeSpan HolidaysHoursAssigned { get; set; }
            public int NightShiftsAssigned { get; set; }
        }

        public sealed class NurseWorkDayRequest
        {
            public ShiftTypes ShiftType { get; set; }
            public int DayNumber { get; set; }
            public TimeOnly ShiftStart { get; set; }
            public TimeOnly ShiftEnd { get; set; }
            public bool IsTimeOff { get; set; }
            public int MorningShiftId { get; set; }
        }

        public sealed class MorningShiftRequest
        {
            public MorningShiftIndex Index { get; set; }
            public TimeSpan Length { get; set; }
        }

        public sealed class DayRequest
        {
            public DateOnly Date { get; set; }
            public bool IsHoliday { get; set; }
        }
    }
}

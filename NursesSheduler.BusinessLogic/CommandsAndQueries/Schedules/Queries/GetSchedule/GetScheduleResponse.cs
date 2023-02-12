using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule
{
    public sealed class GetScheduleResponse
    {
        public int ScheduleId { get; set; }
        public int MonthNumber { get; set; }
        public int MonthInQuarter { get; set; }
        public int Year { get; set; }

        public ICollection<int> Holidays { get; set; }
        public ICollection<ShiftResponse> Shifts { get; set; }
        public ICollection<TimeOff> TimeOffs { get; set; }


        public int DepartamentId { get; set; }

        public sealed class ShiftResponse
        {
            public int Day{ get; set; }
            public ICollection<int> AssignedNursesIds { get; set; }
            public ShiftTypes Type { get; set; }
            public bool IsShortShift { get; set; }
            public TimeOnly ShiftStart { get; set; }
            public TimeOnly ShiftEnd { get; set; }
        }

        public sealed class TimeOff
        {
            public int Day { get; set; }
            public TimeOffTypes Type { get; set; }
            public int NurseId { get; set; }
        }
    }
}

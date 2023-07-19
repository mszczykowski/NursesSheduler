using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.BuildSchedule
{
    public sealed class BuildScheduleResponse
    {
        public int Month { get; set; }
        public bool IsClosed { get; set; }
        public int QuarterId { get; set; }
        public IEnumerable<ScheduleNurseResponse> ScheduleNurses { get; set; }

        public sealed class ScheduleNurseResponse
        {
            public int NurseId { get; init; }
            public int ScheduleId { get; init; }
            public IEnumerable<NurseWorkDayResponse> NurseWorkDays { get; set; }
        }

        public sealed class NurseWorkDayResponse
        {
            public int Day { get; set; }
            public bool IsTimeOff { get; set; }
            public ShiftTypes ShiftType { get; set; }
        }
    }
}

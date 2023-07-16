using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.ScheduleStats.Queries.GetScheduleStatsQuery
{
    public sealed class GetScheduleStatsResponse
    {
        public int MonthInQuarter { get; set; }
        public TimeSpan WorkTimeInMonth { get; set; }
        public TimeSpan WorkTimeBalance { get; set; }
        public IEnumerable<NurseScheduleStatsResponse> NursesScheduleStats { get; set; }

        public sealed class NurseScheduleStatsResponse
        {
            public int NurseId { get; set; }
            public TimeSpan AssignedWorkTime { get; set; }
            public TimeSpan HolidayHoursAssigned { get; set; }
            public TimeSpan TimeOffToAssign { get; set; }
            public TimeSpan TimeOffAssigned { get; set; }
            public int NightShiftsAssigned { get; set; }
            public ShiftTypes LastState { get; set; }
            public int DaysFromLastShift { get; set; }
        }
    }
}

﻿namespace NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStatsFromSchedule
{
    public sealed class GetScheduleStatsFromScheduleResponse
    {
        public int MonthInQuarter { get; set; }
        public TimeSpan WorkTimeInMonth { get; set; }
        public TimeSpan WorkTimeBalance { get; set; }
        public IEnumerable<NurseStatsResponse> NursesScheduleStats { get; set; }
        public sealed class NurseStatsResponse
        {
            public int NurseId { get; set; }
            public TimeSpan AssignedWorkTime { get; set; }
            public TimeSpan HolidayHoursAssigned { get; set; }
            public TimeSpan TimeOffToAssign { get; set; }
            public TimeSpan TimeOffAssigned { get; set; }
            public TimeSpan NightHoursAssigned { get; set; }
        }
    }
}

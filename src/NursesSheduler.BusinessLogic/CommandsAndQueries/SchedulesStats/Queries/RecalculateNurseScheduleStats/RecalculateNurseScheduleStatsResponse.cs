namespace NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.RecalculateNurseScheduleStats
{
    public sealed class RecalculateNursesScheduleStatsResponse
    {
        public int NurseId { get; set; }
        public TimeSpan AssignedWorkTime { get; set; }
        public TimeSpan HolidayHoursAssigned { get; set; }
        public TimeSpan TimeOffToAssign { get; set; }
        public TimeSpan TimeOffAssigned { get; set; }
        public TimeSpan NightHoursAssigned { get; set; }
    }
}

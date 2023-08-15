using NursesScheduler.Domain.Enums;

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
        public IEnumerable<ScheduleValidationErrorResponse> ValidationErrors { get; set; }
        public sealed class ScheduleValidationErrorResponse
        {
            public ScheduleInvalidReasons Reason { get; set; }
            public string? AdditionalInfo { get; set; }
        }
    }
}

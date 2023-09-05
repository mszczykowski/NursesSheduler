using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.NurseStats.Commands.RecalculateNurseStats
{
    public sealed class RecalculateNurseStatsResponse
    {
        public NursesStatsResponse NursesScheduleStats { get; set; }
        public NursesStatsResponse NursesQuarterStats { get; set; }
        public IEnumerable<ScheduleValidationErrorResponse> ValidationErrors { get; set; }
        public sealed class ScheduleValidationErrorResponse
        {
            public int NurseId { get; set; }
            public ScheduleInvalidReasons Reason { get; set; }
            public string? AdditionalInfo { get; set; }
        }

        public sealed class NursesStatsResponse
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

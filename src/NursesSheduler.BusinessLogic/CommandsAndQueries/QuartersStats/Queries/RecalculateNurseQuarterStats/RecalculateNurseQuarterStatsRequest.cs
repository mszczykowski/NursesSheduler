using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.QuartersStats.Queries.RecalculateQuarterStats
{
    internal class RecalculateNurseQuarterStatsRequest
    {
        public IEnumerable<NurseScheduleStatsRequest> NursesScheduleStats { get; set; }
        public sealed class NurseScheduleStatsRequest
        {
            public int NurseId { get; set; }
            public TimeSpan AssignedWorkTime { get; set; }
            public TimeSpan HolidayHoursAssigned { get; set; }
            public TimeSpan TimeOffToAssign { get; set; }
            public TimeSpan TimeOffAssigned { get; set; }
            public int NightShiftsAssigned { get; set; }
        }
    }
}

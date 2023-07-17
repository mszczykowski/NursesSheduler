using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.QuartersStats.Queries.RecalculateNurseQuarterStats
{
    internal class RecalculateNurseQuarterStatsRequest : IRequest<RecalculateNurseQuarterStatsResponse>
    {
        public int CurrentMonth { get; set; }
        public int CurrentYear { get; set; }
        public int DepartamentId { get; set; }
        public NurseStatsRequest CurrentScheduleNurseStats { get; set; }
        public sealed class NurseStatsRequest
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

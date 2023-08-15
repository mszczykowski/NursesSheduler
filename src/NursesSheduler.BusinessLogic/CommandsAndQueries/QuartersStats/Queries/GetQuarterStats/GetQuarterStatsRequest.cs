using MediatR;
using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.QuartersStats.Queries.GetQuarterStats
{
    public sealed class GetQuarterStatsRequest : IRequest<GetQuarterStatsResponse>
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int DepartamentId { get; set; }

        public ScheduleStatsRequest CurrentScheduleStats { get; set; }
        public sealed class ScheduleStatsRequest
        {
            public int MonthInQuarter { get; set; }
            public TimeSpan WorkTimeInMonth { get; set; }
            public TimeSpan WorkTimeBalance { get; set; }
            public IEnumerable<NurseStatsRequest> NursesScheduleStats { get; set; }
        }

        public sealed class NurseStatsRequest
        {
            public int NurseId { get; set; }
            public TimeSpan AssignedWorkTime { get; set; }
            public TimeSpan HolidayHoursAssigned { get; set; }
            public TimeSpan TimeOffToAssign { get; set; }
            public TimeSpan TimeOffAssigned { get; set; }
            public TimeSpan NightHoursAssigned { get; set; }
            public ShiftTypes LastState { get; set; }
            public int DaysFromLastShift { get; set; }
        }
    }
}

using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Queries.GetAbsencesSummary
{
    public class GetAbsencesSummaryResponse
    {
        public int AbsencesSummaryId { get; set; }
        public int Year { get; set; }
        public int PTODays { get; set; }
        public TimeSpan PTOTime { get; set; }
        public TimeSpan PTOTimeUsed { get; set; }
        public TimeSpan PTOTimeLeftFromPreviousYear { get; set; }
        public ICollection<AbsenceResponse> Absences { get; set; }

        public class AbsenceResponse
        {
            public int AbsenceId { get; set; }
            public DateOnly From { get; set; }
            public DateOnly To { get; set; }
            public TimeSpan WorkingHoursToAssign { get; set; }
            public TimeSpan AssignedWorkingHours { get; set; }
            public AbsenceTypes Type { get; set; }
            public int AbsencesSummaryId { get; set; }
        }
    }
}

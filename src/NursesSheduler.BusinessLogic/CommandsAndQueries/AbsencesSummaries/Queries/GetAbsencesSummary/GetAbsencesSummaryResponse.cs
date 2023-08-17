using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Queries.GetAbsencesSummary
{
    public class GetAbsencesSummaryResponse
    {
        public int AbsencesSummaryId { get; set; }
        public int Year { get; set; }
        public TimeSpan PTOTimeLeft { get; set; }
        public TimeSpan PTOTimeUsed { get; set; }
        public TimeSpan PTOTimeLeftFromPreviousYear { get; set; }
        
        public IEnumerable<AbsenceResponse> Absences { get; set; }

        public class AbsenceResponse
        {
            public int AbsenceId { get; set; }
            public int Month { get; set; }
            public ICollection<int> Days { get; set; }
            public TimeSpan WorkTimeToAssign { get; set; }
            public TimeSpan AssignedWorkingHours { get; set; }
            public AbsenceTypes Type { get; set; }
            public int AbsencesSummaryId { get; set; }
        }
    }
}

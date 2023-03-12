using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.YearlyAbsencesSummaries.Queries.GetYearlyAbsencesSummary
{
    public class GetYearlyAbsencesSummaryResponse
    {
        public int YearlyAbsencesSummaryId { get; set; }
        public int Year { get; set; }
        public int PTODays { get; set; }
        public TimeSpan PTO { get; set; }
        public TimeSpan PTOUsed { get; set; }
        public TimeSpan PTOLeftFromPreviousYear { get; set; }
        public ICollection<AbsenceResponse> Absences { get; set; }

        public class AbsenceResponse
        {
            public int AbsenceId { get; set; }
            public DateOnly From { get; set; }
            public DateOnly To { get; set; }
            public TimeSpan AssignedWorkingHours { get; set; }
            public AbsenceTypes Type { get; set; }
            public int YearlyAbsencesSummaryId { get; set; }
        }
    }
}

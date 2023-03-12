using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.YearlyAbsencesSummaries.Commands.AddYearlyAbsencesSummary
{
    public sealed class AddYearlyAbsencesSummaryResponse
    {
        public int YearlyAbsencesId { get; set; }
        public int Year { get; set; }
        public int PTODays { get; set; }
        public TimeSpan PTO { get; set; }
        public TimeSpan PTOUsed { get; set; }
        public TimeSpan PTOLeftFromPreviousYear { get; set; }
        public ICollection<AbsenceResponse> Absences { get; set; }
        public int NurseId { get; set; }

        public sealed class AbsenceResponse
        {
            public int AbsenceId { get; set; }
            public DateOnly From { get; set; }
            public DateOnly To { get; set; }
            public TimeSpan AssignedWorkingHours { get; set; }
            public AbsenceTypes Type { get; set; }
        }
    }
}

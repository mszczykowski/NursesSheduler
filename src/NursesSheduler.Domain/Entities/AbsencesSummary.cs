namespace NursesScheduler.Domain.Entities
{
    public record AbsencesSummary
    {
        public int AbsencesSummaryId { get; set; }
        public int Year { get; set; }
        public TimeSpan PTOTimeLeft { get; set; }
        public TimeSpan PTOTimeLeftFromPreviousYear { get; set; }

        public virtual IEnumerable<Absence> Absences { get; set; }

        public int NurseId { get; set; }
        public virtual Nurse Nurse { get; set; }
    }
}

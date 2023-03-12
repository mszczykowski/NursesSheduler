namespace NursesScheduler.Domain.DomainModels
{
    public class YearlyAbsencesSummary
    {
        public int YearlyAbsencesSummaryId { get; set; }
        public int Year { get; set; }
        public int PTODays { get; set; }
        public TimeSpan PTO { get; set; }
        public TimeSpan PTOUsed { get; set; }
        public TimeSpan PTOLeftFromPreviousYear { get; set; }
        public virtual ICollection<Absence> Absences { get; set; }
        public int NurseId { get; set; }
        public Nurse Nurse { get; set; }
    }
}

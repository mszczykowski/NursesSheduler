namespace NursesScheduler.Domain.DomainModels
{
    public class AbsencesSummary
    {
        public int AbsencesSummaryId { get; set; }
        public int Year { get; set; }
        public TimeSpan PTOTime { get; set; }
        public TimeSpan PTOTimeUsed { get; set; }
        public TimeSpan PTOTimeLeftFromPreviousYear { get; set; }
        
        public virtual ICollection<Absence> Absences { get; set; }
        
        public int NurseId { get; set; }
        public virtual Nurse Nurse { get; set; }
    }
}

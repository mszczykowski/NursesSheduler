using NursesScheduler.Domain.Abstractions;

namespace NursesScheduler.Domain.Entities
{
    public record AbsencesSummary : IEntity
    {
        public int Year { get; set; }
        public TimeSpan PTOTime { get; set; }
        public TimeSpan PTOTimeUsed { get; set; }
        public TimeSpan PTOTimeLeftFromPreviousYear { get; set; }

        public virtual ICollection<Absence> Absences { get; set; }

        public int NurseId { get; set; }
        public virtual Nurse Nurse { get; set; }
    }
}

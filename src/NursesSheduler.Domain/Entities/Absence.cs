using NursesScheduler.Domain.Abstractions;
using NursesScheduler.Domain.Enums;

namespace NursesScheduler.Domain.Entities
{
    public record Absence : IEntity
    {
        public int MonthNumber { get; set; }
        public virtual ICollection<int> Days { get; set; }
        public TimeSpan WorkTimeToAssign { get; set; }
        public TimeSpan AssignedWorkingHours { get; set; }
        public AbsenceTypes Type { get; set; }
        public bool IsClosed { get; set; }

        public int AbsencesSummaryId { get; set; }
        public virtual AbsencesSummary AbsencesSummary { get; set; }

        public Absence(int month)
        {
            MonthNumber = month;
            Days = new HashSet<int>();
        }
    }
}

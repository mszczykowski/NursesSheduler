using NursesScheduler.Domain.Enums;

namespace NursesScheduler.Domain.Entities
{
    public class Absence
    {
        public int AbsenceId { get; set; }
        public int Month { get; set; }
        public ICollection<int> Days { get; set; }
        public TimeSpan WorkTimeToAssign { get; set; }
        public TimeSpan AssignedWorkingHours { get; set; }
        public AbsenceTypes Type { get; set; }
        public bool IsClosed { get; set; }

        public int AbsencesSummaryId { get; set; }
        public virtual AbsencesSummary AbsencesSummary { get; set; }

        public Absence(int month)
        {
            Month = month;
            Days = new HashSet<int>();
        }
    }
}

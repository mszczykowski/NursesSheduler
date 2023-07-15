using NursesScheduler.Domain.Enums;

namespace NursesScheduler.Domain.Entities
{
    public record Absence
    {
        public int AbsenceId { get; set; }
        public int MonthNumber { get; set; }
        public virtual IEnumerable<int> Days { get; set; }
        public TimeSpan WorkTimeToAssign { get; set; }
        public TimeSpan AssignedWorkingHours { get; set; }
        public AbsenceTypes Type { get; set; }
        public bool IsClosed { get; set; }

        public int AbsencesSummaryId { get; set; }
        public virtual AbsencesSummary AbsencesSummary { get; set; }


        public Absence()
        {

        }
        public Absence(int month)
        {
            MonthNumber = month;
            Days = new HashSet<int>();
        }
    }
}

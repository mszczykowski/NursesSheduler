using NursesScheduler.Domain.Enums;

namespace NursesScheduler.Domain.Entities
{
    public class Absence
    {
        public int AbsenceId { get; set; }
        public DateOnly From { get; set; }
        public DateOnly To { get; set; }
        public TimeSpan WorkingHoursToAssign { get; set; }
        public TimeSpan AssignedWorkingHours { get; set; }
        public AbsenceTypes Type { get; set; }

        public int AbsencesSummaryId { get; set; }
        public virtual AbsencesSummary AbsencesSummary { get; set; }

        public virtual ICollection<NurseWorkDay> NurseWorkDays { get; set; }
    }
}

using NursesScheduler.Domain.Enums;

namespace NursesScheduler.Domain.DomainModels
{
    public sealed class Absence
    {
        public int AbsenceId { get; set; }
        public DateOnly From { get; set; }
        public DateOnly To { get; set; }
        public TimeSpan AssignedWorkingHours { get; set; }
        public TimeOffTypes Type { get; set; }

        public int YearlyAbsencesId { get; set; }
        public YearlyAbsencesSummary YearlyAbsences { get; set; }
    }
}

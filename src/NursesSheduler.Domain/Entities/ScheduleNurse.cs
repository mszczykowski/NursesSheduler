using NursesScheduler.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace NursesScheduler.Domain.Entities
{
    public class ScheduleNurse
    {
        public int ScheduleNurseId { get; set; }
        public virtual ICollection<NurseWorkDay> NurseWorkDays { get; set; }

        public int NurseId { get; set; }
        public virtual Nurse Nurse { get; set; }

        public int ScheduleId { get; set; }
        public virtual Schedule Schedule { get; set; }


        [NotMapped]
        public TimeSpan PreviousMonthTime { get; set; }
        [NotMapped]
        public TimeSpan TimeToAssingInMonth { get; set; }
        [NotMapped]
        public TimeSpan TimeOffToAssign { get; set; }
        [NotMapped]
        public TimeSpan TimeToAssingInQuarterLeft { get; set; }
        [NotMapped]
        public PreviousNurseStates PreviousState { get; set; }
        [NotMapped]
        public int DaysFromLastShift { get; set; }
    }
}

using NursesScheduler.Domain.Abstractions;
using NursesScheduler.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace NursesScheduler.Domain.Entities
{
    public record ScheduleNurse : IEntity
    {
        public virtual ICollection<NurseWorkDay> NurseWorkDays { get; set; }

        public int NurseId { get; set; }
        public virtual Nurse Nurse { get; set; }

        public int ScheduleId { get; set; }
        public virtual Schedule Schedule { get; set; }


        public TimeSpan PreviousMonthTime { get; set; }
        public TimeSpan TimeToAssingInMonth { get; set; }
        public TimeSpan TimeOffToAssign { get; set; }
        public TimeSpan TimeToAssingInQuarterLeft { get; set; }
        public TimeSpan HolidaysHoursAssigned { get; set; }
        public int NightShiftsAssigned { get; set; }
        public PreviousNurseStates PreviousState { get; set; }
        public int DaysFromLastShift { get; set; }
    }
}

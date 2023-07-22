namespace NursesScheduler.Domain.Entities
{
    public record ScheduleNurse
    {
        public int ScheduleNurseId { get; set; }
        public virtual IEnumerable<NurseWorkDay> NurseWorkDays { get; set; }

        public int NurseId { get; set; }
        public virtual Nurse Nurse { get; set; }

        public int ScheduleId { get; set; }
        public virtual Schedule Schedule { get; set; }


        public TimeSpan PreviousMonthBalance { get; set; }
        public TimeSpan AssignedWorkTime { get; set; }
        public TimeSpan TimeOffToAssign { get; set; }
        public TimeSpan TimeOffToAssiged { get; set; }
        public TimeSpan HolidaysHoursAssigned { get; set; }
        public TimeSpan NightHoursAssigned { get; set; }
    }
}

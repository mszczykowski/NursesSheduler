namespace NursesScheduler.Domain.Entities
{
    public record Schedule
    {
        public int ScheduleId { get; set; }
        public int Month { get; set; }
        public TimeSpan WorkTimeInMonth { get; set; }
        public TimeSpan WorkTimeBalance { get; set; }
        public TimeSpan TimeOffAssigned { get; set; }
        public bool IsClosed { get; set; }

        public virtual IEnumerable<ScheduleNurse> ScheduleNurses { get; set; }

        public int QuarterId { get; set; }
        public Quarter Quarter { get; set; }
    }
}

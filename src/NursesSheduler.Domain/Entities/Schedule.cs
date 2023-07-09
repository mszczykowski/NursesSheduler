namespace NursesScheduler.Domain.Entities
{
    public record Schedule
    {
        public int ScheduleId { get; set; }
        public int Month { get; set; }
        public int MonthInQuarter { get; set; }
        public int Year { get; set; }
        public TimeSpan WorkTimeInMonth { get; set; }
        public TimeSpan TimeOffAvailableToAssgin { get; set; }
        public TimeSpan TimeOffAssigned { get; set; }
        public bool IsClosed { get; set; }

        public ICollection<int> Holidays { get; set; }
        public virtual ICollection<ScheduleNurse> ScheduleNurses { get; set; }

        public int QuarterId { get; set; }
        public Quarter Quarter { get; set; }

        public int DepartamentId { get; set; }
        public virtual Departament Departament { get; set; }
    }
}

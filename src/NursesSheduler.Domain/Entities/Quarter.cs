namespace NursesScheduler.Domain.Entities
{
    public record Quarter
    {
        public int QuarterId { get; set; }
        public int QuarterNumber { get; set; }
        public int Year { get; set; }
        public int DepartamentId { get; set; }
        public virtual Departament Departament { get; set; }
        public TimeSpan WorkTimeInQuarterToAssign { get; set; }
        public TimeSpan TimeForMorningShifts { get; set; }
        public bool IsClosed { get; set; }
        public int SettingsVersion { get; set; }
        public virtual ICollection<MorningShift> MorningShifts { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}

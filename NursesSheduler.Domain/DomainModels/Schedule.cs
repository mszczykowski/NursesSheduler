namespace NursesScheduler.Domain.DomainModels
{
    public class Schedule
    {
        public int ScheduleId { get; set; }
        public int MonthNumber { get; set; }
        public int MonthInQuarter { get; set; }
        public int Year { get; set; }
        public int Quarter{ get; set; }
        public TimeSpan WorkTimeInMonth { get; set; }
        public TimeSpan PTOTimeAvailableToAssgin { get; set; }
        public TimeSpan PTOTimeAssigned { get; set; }
        public int SettingsVersion { get; set; }

        public virtual ICollection<int> Holidays { get; set; }
        public virtual ICollection<Shift> Shifts { get; set; }


        public int DepartamentId { get; set; }
        public Departament Departament { get; set; }
    }
}

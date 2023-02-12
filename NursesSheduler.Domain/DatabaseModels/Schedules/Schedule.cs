namespace NursesScheduler.Domain.DatabaseModels.Schedules
{
    public sealed class Schedule
    {
        public int ScheduleId { get; set; }
        public int MonthNumber { get; set; }
        public int MonthInQuarter { get; set; }
        public int Year { get; set; }

        public ICollection<int> Holidays { get; set; }
        public ICollection<Shift> Shifts { get; set; }
        public ICollection<TimeOff> TimeOffs { get; set; }


        public int DepartamentId { get; set; }
        public Departament Departament { get; set; }
    }
}

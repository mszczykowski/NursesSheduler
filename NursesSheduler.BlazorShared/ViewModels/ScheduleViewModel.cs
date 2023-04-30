namespace NursesScheduler.BlazorShared.ViewModels
{
    public sealed class ScheduleViewModel
    {
        public int ScheduleId { get; set; }
        public int DepartamentId { get; set; }
        public int MonthNumber { get; set; }
        public int Year { get; set; }
        public int QuarterNumber { get; set; }
        public TimeSpan WorkTimeInMonth { get; set; }
        public TimeSpan WorkTimeInQuarter { get; set; }
        public TimeSpan TimeOffAvailableToAssgin { get; set; }
        public TimeSpan TimeOffAssigned { get; set; }
        public DayViewModel[] MonthDays { get; set; }
        public MorningShiftViewModel[] MorningShifts { get; set; }
        public bool ReadOnly { get; set; }
        public ICollection<ScheduleNurseViewModel> ScheduleNurses { get; set; }
    }
}

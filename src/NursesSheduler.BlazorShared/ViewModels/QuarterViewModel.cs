namespace NursesScheduler.BlazorShared.ViewModels
{
    public sealed class QuarterViewModel
    {
        public int QuarterId { get; set; }
        public int QuarterNumber { get; set; }
        public int Year { get; set; }
        public int DepartamentId { get; set; }
        public TimeSpan WorkTimeInQuarterToAssign { get; set; }
        public TimeSpan TimeForMorningShifts { get; set; }
    }
}

namespace NursesScheduler.BlazorShared.ViewModels
{
    public sealed class QuarterStatsViewModel
    {
        public TimeSpan WorkTimeInQuarter { get; set; }
        public TimeSpan TimeForMorningShifts { get; set; }
        public IDictionary<int, NurseStatsViewModel> NurseStats { get; set; }
    }
}

namespace NursesScheduler.BlazorShared.ViewModels
{
    public sealed class QuarterStatsViewModel
    {
        public TimeSpan WorkTimeInQuarter { get; set; }
        public TimeSpan TimeForMorningShifts { get; set; }
        public IEnumerable<NurseStatsViewModel> NurseStats { get; set; }
    }
}

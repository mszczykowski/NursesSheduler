namespace NursesScheduler.BlazorShared.Models.ViewModels.ValueObjects
{
    public sealed class QuarterStatsViewModel
    {
        public TimeSpan WorkTimeInQuarter { get; set; }
        public TimeSpan TimeForMorningShifts { get; set; }
        public IDictionary<int, NurseStatsViewModel> NurseStats { get; set; }
    }
}

namespace NursesScheduler.BlazorShared.Models.ViewModels.ValueObjects
{
    public sealed class ScheduleStatsViewModel
    {
        public int MonthInQuarter { get; set; }
        public TimeSpan WorkTimeInMonth { get; set; }
        public TimeSpan WorkTimeBalance { get; set; }
        public IDictionary<int, NurseStatsViewModel> NursesScheduleStats { get; set; }

        public TimeSpan AssignedTimeOffsTime => TimeSpan.FromTicks(NursesScheduleStats
            .Sum(s => s.Value.TimeOffAssigned.Ticks));
    }
}

﻿namespace NursesScheduler.BlazorShared.ViewModels
{
    public sealed class ScheduleStatsViewModel
    {
        public int MonthInQuarter { get; set; }
        public TimeSpan WorkTimeInMonth { get; set; }
        public TimeSpan WorkTimeBalance { get; set; }
        public IEnumerable<NurseStatsViewModel> NursesScheduleStats { get; set; }
    }
}

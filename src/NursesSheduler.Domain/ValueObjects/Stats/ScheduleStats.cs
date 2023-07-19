namespace NursesScheduler.Domain.ValueObjects.Stats
{
    public sealed record ScheduleStats
    {
        public ScheduleStatsKey CacheKey { get; set; }
        public int MonthInQuarter { get; set; }
        public TimeSpan WorkTimeInMonth { get; set; }
        public TimeSpan WorkTimeBalance { get; set; }
        public bool IsClosed { get; set; }
        public IEnumerable<NurseScheduleStats> NursesScheduleStats { get; set; }
    }
}

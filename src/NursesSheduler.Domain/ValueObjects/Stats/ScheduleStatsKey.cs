namespace NursesScheduler.Domain.ValueObjects.Stats
{
    public sealed record ScheduleStatsKey
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int DepartamentId { get; set; }

        public override string ToString()
        {
            return $"{Year}-{Month}-{DepartamentId}";
        }
    }
}

namespace NursesScheduler.Domain.ValueObjects
{
    public sealed record WorkTimeInWeek
    {
        public int WeekNumber { get; set; }
        public TimeSpan AssignedWorkTime { get; set; }

        public bool Equals(WorkTimeInWeek? other)
        {
            return other is not null && WeekNumber == other.WeekNumber;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(WeekNumber);
        }
    }
}

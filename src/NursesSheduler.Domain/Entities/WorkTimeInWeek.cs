namespace NursesScheduler.Domain.Entities
{
    public sealed class WorkTimeInWeek
    {
        public int WeekNumber { get; set; }
        public TimeSpan AssignedWorkTime { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is WorkTimeInWeek week &&
                   WeekNumber == week.WeekNumber;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(WeekNumber);
        }
    }
}

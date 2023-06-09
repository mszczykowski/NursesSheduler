namespace NursesScheduler.Domain.Entities
{
    public sealed class WorkTimeInWeek
    {
        public int WorkTimeInWeekId { get; set; }
        public int WeekNumber { get; set; }
        public TimeSpan AssignedWorkTime { get; set; }
    }
}

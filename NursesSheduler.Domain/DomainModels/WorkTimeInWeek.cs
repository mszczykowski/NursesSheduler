namespace NursesScheduler.Domain.DomainModels
{
    public sealed class WorkTimeInWeek
    {
        public int WorkTimeInWeekId { get; set; }
        public int WeekNumber { get; set; }
        public TimeSpan AssignedWorkTime { get; set; }
    }
}

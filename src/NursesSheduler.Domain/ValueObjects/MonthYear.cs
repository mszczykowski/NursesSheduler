namespace NursesScheduler.Domain.ValueObjects
{
    public sealed record MonthYear
    {
        public int Month { get; set; }
        public int Year { get; set; }
    }
}

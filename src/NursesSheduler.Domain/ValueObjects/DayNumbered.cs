namespace NursesScheduler.Domain.ValueObjects
{
    public sealed record DayNumbered : DayNumbered
    {
        public int DayInQuarter { get; set; }
        public int WeekInQuarter => (int)Math.Ceiling((decimal)DayInQuarter / 7) - 1;
    }
}

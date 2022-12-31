namespace NursesScheduler.Domain.Entities.Calendar
{
    public sealed class Month
    {
        public Day[] Days { get; set; }
        public int MonthInQuarter { get; set; }
        public int MonthNumber { get; set; }
        public int Year { get; set; }
    }
}

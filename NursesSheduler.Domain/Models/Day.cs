namespace NursesScheduler.Domain.Models
{
    public sealed class Day
    {
        public DateOnly Date { get; set; }
        public bool IsHoliday { get; set; }
        public string HolidayName { get; set; }
    }
}

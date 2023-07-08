namespace NursesScheduler.Domain.ValueObjects
{
    public sealed record Day
    {
        public DateOnly Date { get; set; }
        public bool IsHoliday { get; set; }
        public string HolidayName { get; set; }
        public bool IsWorkingDay => !IsHoliday && Date.DayOfWeek != DayOfWeek.Saturday &&
            Date.DayOfWeek != DayOfWeek.Sunday;
        public int DayInQuarter { get; set; }
        public int WeekInQuarter => (int)Math.Ceiling((decimal)DayInQuarter / 7) - 1;
        public Day(int day, int month, int year)
        {
            Date = new DateOnly(year, month, day);
        }
    }
}

namespace NursesScheduler.Domain.Entities
{
    public sealed class Day
    {
        public DateOnly Date { get; set; }
        public bool IsHoliday { get; set; }
        public string HolidayName { get; set; }
        public bool IsWorkingDay => !IsHoliday && Date.DayOfWeek != DayOfWeek.Saturday &&
            Date.DayOfWeek != DayOfWeek.Sunday;
        public int DayInQuarter { get; set; }
        public int WeekInQuarter { get; set; }
        public Day(int day, int month, int year)
        {
            Date = new DateOnly(year, month, day);
        }
        public Day(int day, int month, int year, int dayInQuarter)
        {
            Date = new DateOnly(year, month, day);
            DayInQuarter = dayInQuarter;
            WeekInQuarter =  (int)Math.Ceiling((decimal)DayInQuarter / 7) - 1;
        }
    }
}

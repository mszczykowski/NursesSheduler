namespace NursesScheduler.Domain.Entities
{
    public sealed class Day
    {
        public DateOnly Date { get; set; }
        public bool IsHoliday { get; set; }
        public string HolidayName { get; set; }
        public bool IsWorkingDay => !IsHoliday && Date.DayOfWeek != DayOfWeek.Saturday &&
            Date.DayOfWeek != DayOfWeek.Sunday;

        public Day(int day, int month, int year)
        {
            Date = new DateOnly(year, month, day);
        }

        public Day(int day, int month, int year, Holiday holiday)
        {
            IsHoliday = true;
            HolidayName = holiday.LocalName;
            Date = new DateOnly(year, month, day);
        }
    }
}

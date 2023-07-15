namespace NursesScheduler.Domain.ValueObjects
{
    public record Day
    {
        public DateOnly Date { get; set; }
        public bool IsHoliday { get; set; }
        public string HolidayName { get; set; }
        public bool IsWorkingDay => !IsHoliday && Date.DayOfWeek != DayOfWeek.Saturday &&
            Date.DayOfWeek != DayOfWeek.Sunday;
    }
}

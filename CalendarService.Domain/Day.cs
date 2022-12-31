namespace SolverService.Domain.Models.Calendar
{
    public sealed class Day
    {
        public int DayNumber => _date.Day;
        public DayOfWeek DayOfWeek => _date.DayOfWeek;
        public int DayInQuarter { get; }
        public int WeekInQuarter { get; }
        public bool IsHoliday { get; }
        public string? HolidayName { get; }

        private readonly DateTime _date;

        public Day(DateTime date, int dayInQuarter)
        {
            _date = date;
            IsHoliday = false;
            DayInQuarter = dayInQuarter;
            WeekInQuarter = (int)Math.Ceiling((double)dayInQuarter / 7);
        }

        public Day(DateTime date, int dayInQuarter, string holidayName)
        {
            _date = date;
            IsHoliday = true;
            DayInQuarter = dayInQuarter;
            HolidayName = holidayName;
            WeekInQuarter = (int)Math.Ceiling((double)dayInQuarter / 7);
        }
    }
}

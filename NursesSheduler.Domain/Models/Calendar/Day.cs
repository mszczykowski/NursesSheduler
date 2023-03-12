namespace NursesScheduler.Domain.Models.Calendar
{
    public sealed class Day
    {
        public int DayNumber => _date.Day;
        public DayOfWeek DayOfWeek => _date.DayOfWeek;

        private int _dayInQuarter;
        public int DayInQuarter
        {
            get => _dayInQuarter;
            set
            {
                _dayInQuarter = value;
                _weekInQuarter = (int)Math.Ceiling((double)_dayInQuarter / 7);
            }
        }
        private int _weekInQuarter;
        public int WeekInQuarter => _weekInQuarter;
        public bool IsHoliday { get; }
        public string? HolidayName { get; }

        private readonly DateOnly _date;

        public Day(DateOnly date)
        {
            _date = date;
            IsHoliday = false;
        }

        public Day(DateTime date, string holidayName)
        {
            _date = date;
            IsHoliday = true;
            HolidayName = holidayName;
        }
    }
}

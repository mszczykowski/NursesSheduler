namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Days.Queries.GetMonthDays
{
    public sealed class GetMonthDaysResponse
    {
        public bool HolidaysLoaded { get; set; }
        public IEnumerable<DayResponse> MonthDays { get; set; }
        public sealed class DayResponse
        {
            public DateOnly Date { get; set; }
            public bool IsHoliday { get; set; }
            public string HolidayName { get; set; }
        }
    }
}

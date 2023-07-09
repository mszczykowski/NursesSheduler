namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Days.Queries.GetMonthDays
{
    public sealed class GetMonthDaysResponse
    {
        public DateOnly Date { get; set; }
        public bool IsHoliday { get; set; }
        public string HolidayName { get; set; }
    }
}

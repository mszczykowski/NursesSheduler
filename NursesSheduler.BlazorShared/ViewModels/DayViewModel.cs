using NursesScheduler.BlazorShared.ViewModels.Enums;

namespace NursesScheduler.BlazorShared.ViewModels
{
    public sealed class DayViewModel
    {
        public DateOnly Date { get; set; }
        public bool IsHoliday { get; set; }
        public string HolidayName { get; set; }

        public string GetDayOfWeekAbreviation()
        {
            return ((DayOfWeekAbreviations)Date.DayOfWeek).ToString() + ".";
        }

        public override string ToString()
        {
            return $"{Date.Day}.{Date.Month.ToString().PadLeft(2, '0')}";
        }
    }
}

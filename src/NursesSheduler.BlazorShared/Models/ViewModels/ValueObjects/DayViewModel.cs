using NursesScheduler.BlazorShared.Models.Enums;

namespace NursesScheduler.BlazorShared.Models.ViewModels.ValueObjects
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

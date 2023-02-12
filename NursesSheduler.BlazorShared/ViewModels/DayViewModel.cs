using NursesScheduler.BlazorShared.ViewModels.Enums;

namespace NursesScheduler.BlazorShared.ViewModels
{
    public sealed class DayViewModel
    {
        public DayOfWeek DayOfWeek { get; set; }
        public int DayOfMonth { get; set; }
        public DayType DayType { get; set; }

        public string GetDayOfWeekAbreviation()
        {
            return ((DayOfWeekAbreviations)DayOfWeek).ToString() + ".";
        }
    }
}

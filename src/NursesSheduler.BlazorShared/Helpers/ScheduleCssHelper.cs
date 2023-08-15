using NursesScheduler.BlazorShared.Models.ViewModels.ValueObjects;

namespace NursesScheduler.BlazorShared.Helpers
{
    internal static class ScheduleCssHelper
    {
        public static string GetDayClass(DayViewModel dayViewModel)
        {
            if(dayViewModel == null)
                return String.Empty;

            if(dayViewModel.IsHoliday)
                return "holiday sunday";

            if (dayViewModel.Date.DayOfWeek == DayOfWeek.Saturday)
                return "saturday";

            if (dayViewModel.Date.DayOfWeek == DayOfWeek.Sunday)
                return "sunday";

            return String.Empty;
        }

        public static string SetIsTimeOff(bool isTimeOff)
        {
            if (isTimeOff) return "time-off";
            return String.Empty;
        }
    }
}

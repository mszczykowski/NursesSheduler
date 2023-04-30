using NursesScheduler.BlazorShared.ViewModels;

namespace NursesScheduler.BlazorShared.Helpers
{
    internal static class ScheduleCssHelper
    {
        public static string GetDayClass(DayViewModel dayViewModel)
        {
            if(dayViewModel == null)
                return "";

            if(dayViewModel.IsHoliday)
                return "holiday sunday";

            if (dayViewModel.Date.DayOfWeek == DayOfWeek.Saturday)
                return "saturday";

            if (dayViewModel.Date.DayOfWeek == DayOfWeek.Sunday)
                return "sunday";

            return "";
        }
    }
}

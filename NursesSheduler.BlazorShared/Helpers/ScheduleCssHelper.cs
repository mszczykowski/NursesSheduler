using NursesScheduler.BlazorShared.ViewModels;
using NursesScheduler.BlazorShared.ViewModels.Enums;

namespace NursesScheduler.BlazorShared.Helpers
{
    public static class ScheduleCssHelper
    {
        public static string GetDayClass(DayViewModel dayViewModel)
        {
            switch(dayViewModel.DayType)
            {
                case DayType.Work_free:
                    return "day holiday";
                case DayType.Holiday:
                    return "day free-day";
                default:
                    return "day";
            }
        }
    }
}

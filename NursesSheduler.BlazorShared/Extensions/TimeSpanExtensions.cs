namespace NursesScheduler.BlazorShared.Extensions
{
    internal static class TimeSpanExtensions
    {
        public static string GetTotalHoursAndMinutes(this TimeSpan timeSpan)
        {
            return Math.Floor(timeSpan.TotalHours).ToString().PadLeft(2, '0') + ":" + 
                                                            timeSpan.Minutes.ToString().PadLeft(2, '0');
        }
    }
}

namespace NursesScheduler.BusinessLogic.Extensions
{
    public static class EnumerableExtensions
    {
        public static TimeSpan SumTimeSpan<T>(this IEnumerable<T> objects, Func<T, TimeSpan> selector) => 
            objects.Select(selector).SumTimeSpan();

        public static TimeSpan SumTimeSpan(this IEnumerable<TimeSpan>? source)
        {
            if (source is null)
            {
                throw new InvalidOperationException("Cannot compute timespan sum for a null set.");
            }

            return source.Aggregate(TimeSpan.Zero, (current, next) => current + next);
        }
    }
}
 
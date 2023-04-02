using NursesScheduler.BusinessLogic.Abstractions.Services;

namespace NursesScheduler.BusinessLogic.Services
{
    public class CurrentDateService : ICurrentDateService
    {
        public DateOnly GetCurrentDate() => DateOnly.FromDateTime(DateTime.Now);
    }
}

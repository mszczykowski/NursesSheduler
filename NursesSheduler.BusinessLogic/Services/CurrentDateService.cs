using NursesScheduler.BusinessLogic.Abstractions.Services;

namespace NursesScheduler.BusinessLogic.Services
{
    internal class CurrentDateService : ICurrentDateService
    {
        public DateOnly GetCurrentDate() => DateOnly.FromDateTime(DateTime.Now);
    }
}

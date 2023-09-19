using NursesScheduler.BusinessLogic.Abstractions.Services;

namespace NursesScheduler.BusinessLogic.Services
{
    internal sealed class SeedService : ISeedService
    {
        public string GetSeed() => Environment.TickCount.ToString();
    }
}

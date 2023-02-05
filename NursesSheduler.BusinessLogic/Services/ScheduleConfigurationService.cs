using NursesScheduler.BusinessLogic.Interfaces.Services;

namespace NursesScheduler.BusinessLogic.Services
{
    public sealed class ScheduleConfigurationService : IScheduleConfigurationService
    {
        public int GetQuarterStart()
        {
            return 2;
        }
    }
}

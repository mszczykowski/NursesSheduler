using NursesScheduler.BusinessLogic.Abstractions.Managers;

namespace NursesScheduler.BusinessLogic.Managers
{
    internal class WorkTimeConfigurationManager : IWorkTimeConfigurationManager
    {
        public async Task<TimeSpan> GetShiftLenght()
        {
            await Task.Delay(1);
            return new TimeSpan(12, 0, 0);
        }

        public async Task<TimeSpan> GetWorkDayLenght()
        {
            await Task.Delay(1);
            return new TimeSpan(7, 35, 0);
        }
    }
}

using NursesScheduler.Domain.Models.Settings;

namespace NursesScheduler.BusinessLogic.Abstractions.Managers
{
    internal interface IWorkTimeConfigurationManager
    {
        Task<TimeSpan> GetWorkDayLenght();
        Task<TimeSpan> GetShiftLenght();
    }
}

using NursesScheduler.Domain.DomainModels;

namespace NursesScheduler.BusinessLogic.Abstractions.Services
{
    internal interface IWorkTimeService
    {
        Task<TimeSpan> GetTotalWorkingHours(DateOnly from, DateOnly to);
    }
}
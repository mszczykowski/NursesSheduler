using NursesScheduler.Domain.Models;

namespace NursesScheduler.BusinessLogic.Abstractions.Infrastructure
{
    public interface IHolidaysApiClient
    {
        Task<List<Holiday>?> GetHolidays(int year);
    }
}
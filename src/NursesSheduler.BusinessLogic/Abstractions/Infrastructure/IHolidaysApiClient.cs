using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Abstractions.Infrastructure
{
    public interface IHolidaysApiClient
    {
        Task<IEnumerable<Holiday>> GetHolidays(int year);
    }
}
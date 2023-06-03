using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Abstractions.Infrastructure
{
    public interface IHolidaysApiClient
    {
        Task<List<Holiday>> GetHolidays(int year);
    }
}
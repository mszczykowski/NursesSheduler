using NursesScheduler.Domain.Entities.Calendar;

namespace NursesScheduler.Infrastructure.Interfaces
{
    public interface IHolidaysApiClient
    {
        Task<List<Holiday>?> GetHolidays(int year);
    }
}
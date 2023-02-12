using NursesScheduler.Domain.Models.Calendar;

namespace NursesScheduler.BusinessLogic.Interfaces.Infrastructure
{
    public interface IHolidaysApiClient
    {
        Task<List<Holiday>?> GetHolidays(int year);
    }
}
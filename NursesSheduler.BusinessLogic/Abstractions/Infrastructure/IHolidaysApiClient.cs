using NursesScheduler.Domain.Models.Calendar;

namespace NursesScheduler.BusinessLogic.Abstractions.Infrastructure
{
    public interface IHolidaysApiClient
    {
        Task<List<Holiday>?> GetHolidays(int year);
    }
}
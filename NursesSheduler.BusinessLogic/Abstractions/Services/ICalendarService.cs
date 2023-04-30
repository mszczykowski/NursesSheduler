using NursesScheduler.Domain.Models;

namespace NursesScheduler.BusinessLogic.Abstractions.Services
{
    public interface ICalendarService
    {
        Task<ICollection<Holiday>> GetHolidaysInMonth(int monthNumber, int yearNumber);
        Task<Day[]> GetMonthDays(int monthNumber, int yearNumber);
    }
}

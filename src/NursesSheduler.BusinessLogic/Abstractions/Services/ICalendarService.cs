using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Abstractions.Services
{
    public interface ICalendarService
    {
        Task<IEnumerable<DayNumbered>> GetMonthDaysAsync(int year, int month);
        Task<IEnumerable<DayNumbered>> GetNumberedMonthDaysAsync(int year, int month, int firstQuarterStart);
        Task<IEnumerable<DayNumbered>> GetDaysFromDayNumbersAsync(int year, int month, IEnumerable<int> days);


        IEnumerable<(int year, int month)> GetQuarterMonths(int year, int quarterNumber, int firstQuarterStart);
        int GetMonthInQuarterNumber(int month, int firstQuarterStart);
        int GetQuarterNumber(int month, int firstQuarterStart);
    }
}

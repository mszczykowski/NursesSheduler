using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Abstractions.Services
{
    public interface ICalendarService
    {
        Task<IEnumerable<Day>> GetMonthDaysAsync(int year, int month);
        Task<IEnumerable<DayNumbered>> GetMonthDaysAsync(int year, int month, int firstQuarterStart);
        Task<IEnumerable<Day>> GetDaysFromDayNumbersAsync(int year, int month, ICollection<int> days);
        

        IEnumerable<MonthYear> GetQuarterMonths(int year, int quarterNumber, int firstQuarterStart);
        int GetMonthInQuarterNumber(int month, int firstQuarterStart);
        int GetQuarterNumber(int month, int firstQuarterStart);
    }
}

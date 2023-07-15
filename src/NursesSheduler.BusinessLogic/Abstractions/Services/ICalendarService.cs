using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Abstractions.Services
{
    public interface ICalendarService
    {
        Task<Day[]> GetMonthDaysAsync(int monthNumber, int yearNumber, int firstQuarterStart);
        Task<ICollection<Day>> GetDaysFromDayNumbersAsync(int monthNumber, int yearNumber,
            ICollection<int> dayNumbers);
        ICollection<DateOnly> GetMonthsInQuarterDatesAsync(int quarterStart, int quarterNumber, int year);
        Task<Day[]> GetMonthDaysAsync(int monthNumber, int yearNumber);

        int GetQuarterNumber(int monthNumber, int firstQuarterStart);
        int GetMonthInQuarterNumber(int monthNumber, int firstQuarterStart);
    }
}

using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Abstractions.Services
{
    public interface ICalendarService
    {
        Task<Day[]> GetMonthDays(int monthNumber, int yearNumber, int firstQuarterStart);
        Task<ICollection<Day>> GetDaysFromDayNumbers(int monthNumber, int yearNumber,
            ICollection<int> dayNumbers);
        ICollection<DateOnly> GetMonthsInQuarterDates(int quarterStart, int quarterNumber, int year);
        int GetQuarterNumber(int monthNumber, int firstQuarterStart);
        Task<Day[]> GetMonthDays(int monthNumber, int yearNumber);
    }
}

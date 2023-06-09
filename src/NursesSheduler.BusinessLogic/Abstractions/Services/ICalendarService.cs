using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Abstractions.Services
{
    public interface ICalendarService
    {
        Task<ICollection<Holiday>> GetHolidaysInMonth(int monthNumber, int yearNumber);
        Task<Day[]> GetMonthDays(int monthNumber, int yearNumber);
        Task<ICollection<Day>> GetDaysFromDayNumbers(int monthNumber, int yearNumber,
            ICollection<int> dayNumbers);
    }
}

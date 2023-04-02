using NursesScheduler.Domain.Models.Calendar;

namespace NursesScheduler.BusinessLogic.Abstractions.Services
{
    public interface ICalendarService
    {
        Task<Quarter> GetQuarter(int whichQuarter, int year);
        Task<Month> GetMonth(int monthNumber, int yearNumber);
    }
}

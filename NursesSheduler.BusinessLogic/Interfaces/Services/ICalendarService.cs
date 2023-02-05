using NursesScheduler.Domain.Entities.Calendar;

namespace NursesScheduler.BusinessLogic.Interfaces.Services
{
    public interface ICalendarService
    {
        Task<Quarter> GetQuarter(int whichQuarter, int year);
    }
}

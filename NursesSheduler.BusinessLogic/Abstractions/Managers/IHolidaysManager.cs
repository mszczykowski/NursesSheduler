using NursesScheduler.Domain.Models;

namespace NursesScheduler.BusinessLogic.Abstractions.Managers
{
    internal interface IHolidaysManager
    {
        Task<ICollection<Holiday>> GetHolidays(int year);
    }
}
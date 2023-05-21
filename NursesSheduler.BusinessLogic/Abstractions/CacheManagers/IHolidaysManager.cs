using NursesScheduler.Domain.Models;

namespace NursesScheduler.BusinessLogic.Abstractions.CacheManagers
{
    internal interface IHolidaysManager
    {
        Task<ICollection<Holiday>> GetHolidays(int year);
    }
}
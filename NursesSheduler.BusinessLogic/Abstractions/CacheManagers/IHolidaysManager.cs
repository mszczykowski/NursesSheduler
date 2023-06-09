using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Abstractions.CacheManagers
{
    internal interface IHolidaysManager
    {
        Task<ICollection<Holiday>> GetHolidays(int year);
    }
}
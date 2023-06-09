using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Abstractions.CacheManagers
{
    internal interface IDepartamentSettingsManager
    {
        Task<DepartamentSettings> GetDepartamentSettings(int departamentId);
        void InvalidateCache(int departamentId);
    }
}
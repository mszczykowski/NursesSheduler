using NursesScheduler.Domain.DomainModels;

namespace NursesScheduler.BusinessLogic.Abstractions.Managers
{
    internal interface IDepartamentSettingsManager
    {
        Task<DepartamentSettings> GetDepartamentSettings(int departamentId);
        void InvalidateCache(int departamentId);
    }
}
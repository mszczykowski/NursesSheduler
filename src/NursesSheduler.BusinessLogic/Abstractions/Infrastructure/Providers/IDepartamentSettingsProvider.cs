using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Abstractions.Infrastructure.Providers
{
    public interface IDepartamentSettingsProvider
    {
        Task<DepartamentSettings> GetDepartamentSettings(int departamentId);
        void InvalidateCache(int departamentId);
    }
}
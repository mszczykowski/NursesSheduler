using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Abstractions.Infrastructure.Providers
{
    public interface IDepartamentSettingsProvider : ICacheProvider<DepartamentSettings, int>
    {

    }
}
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Abstractions.Infrastructure.Providers
{
    public interface IHolidaysProvider : ICacheProvider<IEnumerable<Holiday>, int>
    {

    }
}
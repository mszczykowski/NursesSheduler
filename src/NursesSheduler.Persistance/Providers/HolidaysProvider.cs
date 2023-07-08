using Microsoft.Extensions.Caching.Memory;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure.Providers;
using NursesScheduler.BusinessLogic.Exceptions;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.Infrastructure.Providers
{
    public sealed class HolidaysProvider : IHolidaysProvider
    {
        private const string HOLIDAYS_CACHE_KEY = "Holidays-";

        private readonly IHolidaysApiClient _holidaysApiClient;
        private readonly IMemoryCache _memoryCache;

        public HolidaysProvider(IHolidaysApiClient holidaysApiClient, IMemoryCache memoryCache)
        {
            _holidaysApiClient = holidaysApiClient;
            _memoryCache = memoryCache;
        }

        public async Task<ICollection<Holiday>> GetHolidays(int year)
        {
            ICollection<Holiday> result;
            var key = $"{HOLIDAYS_CACHE_KEY}{year}";

            if (!_memoryCache.TryGetValue(key, out result))
            {
                result = await _holidaysApiClient.GetHolidays(year);

                if (result == null || result.Count == 0)
                    throw new EntityNotFoundException(year, nameof(Holiday));

                _memoryCache.Set(key, result);
            }
            return result;
        }
    }
}

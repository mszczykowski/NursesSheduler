using Microsoft.Extensions.Caching.Memory;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure.Providers;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.Infrastructure.Providers
{
    public sealed class HolidaysProvider : CacheProvider<IEnumerable<Holiday>, int>, IHolidaysProvider
    {
        private const string HOLIDAYS_CACHE_KEY = "Holidays";

        private readonly IHolidaysApiClient _holidaysApiClient;

        public HolidaysProvider(IHolidaysApiClient holidaysApiClient, IMemoryCache memoryCache) 
            : base(memoryCache, HOLIDAYS_CACHE_KEY)
        {
            _holidaysApiClient = holidaysApiClient;
        }

        protected override async Task<IEnumerable<Holiday>?> GetDataFromSource(int key)
        {
            return await _holidaysApiClient.GetHolidays(key);
        }
    }
}

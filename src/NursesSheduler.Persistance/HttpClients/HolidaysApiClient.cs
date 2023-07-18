using NursesScheduler.Domain;
using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Memory;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.Domain.ValueObjects;
using NursesScheduler.Domain.Exceptions;

namespace NursesScheduler.Infrastructure.HttpClients
{
    public sealed class HolidaysApiClient : IHolidaysApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _memoryCache;

        public HolidaysApiClient(HttpClient httpClient, IMemoryCache memoryCache)
        {
            _httpClient = httpClient;
            _memoryCache = memoryCache;
        }

        public async Task<IEnumerable<Holiday>> GetHolidays(int year)
        {
            List<Holiday> result;
            var key = $"Holidays-{year}";

            if(!_memoryCache.TryGetValue(key, out result))
            {
                result = await _httpClient.GetFromJsonAsync<List<Holiday>>($"{year}/{GeneralConstants.CountryCode}");

                if (result is null)
                {
                    throw new EntityNotFoundException("Holidays not loaded");
                }

                _memoryCache.Set(key, result, DateTime.Now.AddDays(1));
            }
            return result;
        }
    }
}

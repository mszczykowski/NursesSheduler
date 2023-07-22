using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Memory;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.Domain.ValueObjects;
using NursesScheduler.Domain.Exceptions;
using NursesScheduler.Domain.Constants;

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
            var result = await _httpClient.GetFromJsonAsync<List<Holiday>>($"{year}/{GeneralConstatns.CountryCode}")
                ?? throw new EntityNotFoundException("Holidays not loaded");

            return result;
        }
    }
}

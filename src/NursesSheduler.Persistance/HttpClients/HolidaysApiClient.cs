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

        public HolidaysApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Holiday>> GetHolidays(int year)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Holiday>>($"{year}/{GeneralConstatns.CountryCode}");
            }
            catch(Exception e)
            {
                return Enumerable.Empty<Holiday>();
            }
        }
    }
}

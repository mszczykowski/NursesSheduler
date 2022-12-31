using NursesScheduler.Domain.Entities.Calendar;
using SheduleSolver.Domain.Models.Calendar;
using System.Net.Http.Headers;

namespace SolverService.Implementation.Services
{
    internal sealed class HoliadyApiClient
    {
        private const string countryCode = "PL";

        private const string URL = "https://date.nager.at/api/v3/publicholidays/";

        private readonly HttpClient _httpClient; //should use IHttpClientBuilder

        private int quaterStart = 2;

        private int quarterIterator;

        public HoliadyApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(URL);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<Holiday>?> GetHolidays(int year)
        {
            return await _httpClient.GetFromJsonAsync<List<Holiday>>(year + "/" + countryCode);
        }

        public async Task<Quarter> GetQuarter(int whichQuarter, int quarterStartYear)
        {
            var currentDate = DateTime.Now;

            Quarter quarter = new Quarter();
            quarter.Months = new Month[3];

            quarterIterator = 0;
            int month;

            for (int i = 0; i < 3; i++)
            {
                month = quaterStart + i + whichQuarter * 3;
                if (month > 12)
                {
                    month = 1;
                    quarterStartYear++;
                    holidays = null;
                }
                quarter.Months[i] = await GetMonth(month, quarterStartYear);
                quarter.Months[i].MonthInQuarter = i + 1;
            }

            return quarter;
        }

        public async Task<Month> GetMonth(int monthNumber, int yearNumber)
        {
            if (holidays == null) holidays = await GetHolidays(yearNumber);

            List<Holiday> holidaysInRequestedMonth = holidays.Where(h => h.Date.Month == monthNumber).ToList();

            int daysInMonth = DateTime.DaysInMonth(yearNumber, monthNumber);

            Month month = new Month();
            month.MonthNumber = monthNumber;
            month.Year = yearNumber;

            month.Days = new Day[daysInMonth];

            holidaysInRequestedMonth.ForEach(holiday =>
            {
                month.Days[holiday.Date.Day - 1] = new Day(holiday.Date, quarterIterator + holiday.Date.Day, holiday.Name);
            });

            for (int i = 0; i < daysInMonth; i++)
            {
                quarterIterator++;
                if (month.Days[i] == null)
                {
                    month.Days[i] = new Day(new DateTime(yearNumber, monthNumber, i + 1), quarterIterator);
                }
            }

            return month;
        }
    }
}

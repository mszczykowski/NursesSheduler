using Microsoft.Extensions.Caching.Memory;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.Models.Calendar;

namespace NursesScheduler.BusinessLogic.Services
{
    public sealed class CalendarService : ICalendarService
    {
        private readonly IScheduleConfigurationService _scheduleConfiguration;
        private readonly IHolidaysApiClient _holidaysApiClient;
        private readonly IMemoryCache _memoryCache;

        private ICollection<Holiday> _holidays;

        public CalendarService(IHolidaysApiClient holidaysApiClient, IScheduleConfigurationService scheduleConfiguration, 
            IMemoryCache memoryCache)
        {
            _holidaysApiClient = holidaysApiClient;
            _scheduleConfiguration = scheduleConfiguration;
            _memoryCache = memoryCache;
        }

        public async Task<Quarter> GetQuarter(int whichQuarter, int year)
        {
            Quarter quarter = new Quarter();

            int quarterStart = _scheduleConfiguration.GetQuarterStart();

            int month;
            int dayInQuarterIterator = 1;

            for (int i = 0; i < 3; i++)
            {
                month = quarterStart + i + whichQuarter * 3;
                if (month > 12)
                {
                    month = 1;
                    year++;
                }
                quarter.Months[i] = await GetMonth(month, year);
                quarter.Months[i].MonthInQuarter = i + 1;

                foreach(var day in quarter.Months[i].Days)
                {
                    day.DayInQuarter = dayInQuarterIterator++;
                }
            }

            return quarter;
        }

        public async Task<Month> GetMonth(int monthNumber, int yearNumber)
        {
            Month monthResult;
            var key = $"Month-{monthNumber}.{yearNumber}";

            if(!_memoryCache.TryGetValue(key, out monthResult))
            {
                if (_holidays == null || _holidays.Any() && _holidays.First().Date.Year != yearNumber)
                    _holidays = await _holidaysApiClient.GetHolidays(yearNumber);

                List<Holiday> holidaysInRequestedMonth = _holidays.Where(h => h.Date.Month == monthNumber).ToList();

                int daysInMonth = DateTime.DaysInMonth(yearNumber, monthNumber);

                monthResult = new Month();
                monthResult.MonthNumber = monthNumber;
                monthResult.Year = yearNumber;

                monthResult.Days = new Day[daysInMonth];

                holidaysInRequestedMonth.ForEach(holiday =>
                {
                    monthResult.Days[holiday.Date.Day - 1] = new Day(DateOnly.FromDateTime(holiday.Date), holiday.Name);
                });

                for (int i = 0; i < daysInMonth; i++)
                {
                    if (monthResult.Days[i] == null)
                    {
                        monthResult.Days[i] = new Day(new DateOnly(yearNumber, monthNumber, i + 1));
                    }
                }

                _memoryCache.Set(key, monthResult, DateTime.Now.AddDays(1));
            }

            return monthResult;
        }
    }
}

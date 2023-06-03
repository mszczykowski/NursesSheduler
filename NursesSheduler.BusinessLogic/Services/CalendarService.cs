using NursesScheduler.BusinessLogic.Abstractions.CacheManagers;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Services
{
    internal sealed class CalendarService : ICalendarService
    {
        private readonly IHolidaysManager _holidaysManager;

        public CalendarService(IHolidaysManager holidaysManager)
        {
            _holidaysManager = holidaysManager;
        }

        public async Task<ICollection<Holiday>> GetHolidaysInMonth(int monthNumber, int yearNumber)
        {
            var holidays = await _holidaysManager.GetHolidays(yearNumber);

            return holidays.Where(h => h.Date.Month == monthNumber).ToList();
        }

        public async Task<Day[]> GetMonthDays(int monthNumber, int yearNumber)
        {
            var holidays = await _holidaysManager.GetHolidays(yearNumber);

            holidays = holidays.Where(h => h.Date.Month == monthNumber).ToList();

            var monthDays = new Day[DateTime.DaysInMonth(yearNumber, monthNumber)];

            for(int i = 0; i < monthDays.Length; i++)
            {
                monthDays[i] = new Day
                {
                    Date = new DateOnly(yearNumber, monthNumber, i + 1),
                };
            }

            foreach(var holiday in holidays)
            {
                monthDays[holiday.Date.Day - 1].IsHoliday = true;
                monthDays[holiday.Date.Day - 1].HolidayName = holiday.LocalName;
            }

            return monthDays;
        }

        /*public async Task<Quarter> GetQuarter(int whichQuarter, int year)
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
        }*/
    }
}

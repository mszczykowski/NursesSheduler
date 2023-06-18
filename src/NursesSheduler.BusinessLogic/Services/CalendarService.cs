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

        public async Task<Day[]> GetMonthDays(int monthNumber, int yearNumber, int firstQuarterStart)
        {
            var quarterNumber = GetQuarterNumber(monthNumber, firstQuarterStart);
            var quarterMonthDates = GetMonthsInQuarterDates(firstQuarterStart, quarterNumber, yearNumber);
            var dayInQuarterOffset = 0;

            foreach(var monthDate in quarterMonthDates)
            {
                if(monthDate.Month < monthNumber)
                {
                    dayInQuarterOffset += DateTime.DaysInMonth(monthDate.Year, monthDate.Month);
                }
            }


            var holidays = await _holidaysManager.GetHolidays(yearNumber);

            holidays = holidays.Where(h => h.Date.Month == monthNumber).ToList();

            var monthDays = new Day[DateTime.DaysInMonth(yearNumber, monthNumber)];

            for(int i = 0; i < monthDays.Length; i++)
            {
                monthDays[i] = new Day(i + 1, monthNumber, yearNumber, dayInQuarterOffset + i);
            }

            foreach(var holiday in holidays)
            {
                monthDays[holiday.Date.Day - 1].IsHoliday = true;
                monthDays[holiday.Date.Day - 1].HolidayName = holiday.LocalName;
            }

            return monthDays;
        }

        public async Task<ICollection<Day>> GetDaysFromDayNumbers(int monthNumber, int yearNumber, 
            ICollection<int> dayNumbers)
        {
            var holidays = await _holidaysManager.GetHolidays(yearNumber);

            holidays = holidays.Where(h => h.Date.Month == monthNumber).ToList();

            var daysResult = new HashSet<Day>();

            foreach(var dayNumber in dayNumbers)
            {
                daysResult.Add(new Day(dayNumber, monthNumber, yearNumber));
            }

            foreach (var holiday in holidays)
            {
                var day = daysResult.FirstOrDefault(d => d.Date.Day == holiday.Date.Day);
                if (day != null)
                {
                    day.IsHoliday = true;
                    day.HolidayName = holiday.LocalName;
                }
            }

            return daysResult;
        }

        public ICollection<DateOnly> GetMonthsInQuarterDates(int firstQuarterStart, int quarterNumber, int year)
        {
            var result = new List<DateOnly>();

            for (int i = 0; i < 3; i++)
            {
                var month = firstQuarterStart + i + quarterNumber * 3;
                if (month > 12)
                {
                    month = 1;
                    year++;
                }

                result.Add(new DateOnly(year, month, 1));
            }

            return result;
        }

        public int GetQuarterNumber(int monthNumber, int firstQuarterStart)
        {
            var relativeMonthNumber = 1;

            for (int i = firstQuarterStart; i != monthNumber; i = (i % 12) + 1)
            {
                relativeMonthNumber++;
            }

            return (int)(Math.Ceiling((decimal)(relativeMonthNumber) / 3));
        }
    }
}

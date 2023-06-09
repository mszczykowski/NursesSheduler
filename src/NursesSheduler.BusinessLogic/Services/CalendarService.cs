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
                monthDays[i] = new Day(i + 1, monthNumber, yearNumber);
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
    }
}

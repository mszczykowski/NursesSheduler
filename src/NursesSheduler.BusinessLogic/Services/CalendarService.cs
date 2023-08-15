using NursesScheduler.BusinessLogic.Abstractions.Infrastructure.Providers;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Services
{
    internal sealed class CalendarService : ICalendarService
    {
        private readonly IHolidaysProvider _holidaysProvider;

        public CalendarService(IHolidaysProvider holidaysProvider)
        {
            _holidaysProvider = holidaysProvider;
        }

        public async Task<IEnumerable<DayNumbered>> GetMonthDaysAsync(int year, int month)
        {
            return await GetNumberedDaysAsync(year, month);
        }

        public async Task<IEnumerable<DayNumbered>> GetNumberedMonthDaysAsync(int year, int month, int firstQuarterStart)
        {
            var quarterNumber = GetQuarterNumber(month, firstQuarterStart);
            var quarterMonths = GetQuarterMonths(year, quarterNumber, firstQuarterStart);
            var dayInQuarterOffset = 1;

            foreach (var monthDate in quarterMonths)
            {
                if (monthDate.Month == month && monthDate.Year == year)
                {
                    break;
                }

                dayInQuarterOffset += DateTime.DaysInMonth(monthDate.Year, monthDate.Month);
            }

            var monthDays = await GetNumberedDaysAsync(year, month);

            for (int i = 0; i < monthDays.Length; i++)
            {
                monthDays[i].DayInQuarter = dayInQuarterOffset + i;
            }

            return monthDays;
        }

        public async Task<IEnumerable<DayNumbered>> GetDaysFromDayNumbersAsync(int year, int month,
            IEnumerable<int> days)
        {
            var holidays = await _holidaysProvider.GetCachedDataAsync(year);
            holidays = holidays.Where(h => h.Date.Month == month);

            var daysResult = new List<DayNumbered>();

            foreach (var day in days)
            {
                daysResult.Add(new DayNumbered
                {
                    Date = new DateOnly(year, month, day),
                });
            }

            foreach (var holiday in holidays)
            {
                var day = daysResult.FirstOrDefault(d => d.Date.Day == holiday.Date.Day);

                if (day is not null)
                {
                    day.IsHoliday = true;
                    day.HolidayName = holiday.LocalName;
                }
            }

            return daysResult;
        }

        public IEnumerable<(int Year, int Month)> GetQuarterMonths(int year, int quarterNumber, int firstQuarterStart)
        {
            var result = new List<(int Year, int Month)>();

            for (int i = 0; i < 3; i++)
            {
                var month = firstQuarterStart + i + (quarterNumber - 1) * 3;
                if (month > 12)
                {
                    month = 1;
                    year++;
                }

                result.Add((year, month));
            }

            return result;
        }

        public int GetMonthInQuarterNumber(int month, int firstQuarterStart)
        {
            var monthInQuarter = 1;

            for (int i = firstQuarterStart; i != month; i = (i % 12) + 1)
            {
                monthInQuarter++;
                if (monthInQuarter > 3)
                {
                    monthInQuarter = 1;
                }
            }

            return monthInQuarter;
        }

        public int GetQuarterNumber(int month, int firstQuarterStart)
        {
            var relativeMonthNumber = 1;

            for (int i = firstQuarterStart; i != month; i = (i % 12) + 1)
            {
                relativeMonthNumber++;
            }

            return (int)(Math.Ceiling((decimal)(relativeMonthNumber) / 3));
        }

        private async Task<DayNumbered[]> GetNumberedDaysAsync(int year, int month)
        {
            var holidays = await _holidaysProvider.GetCachedDataAsync(year);
            holidays = holidays.Where(h => h.Date.Month == month);

            var monthDays = new DayNumbered[DateTime.DaysInMonth(year, month)];

            for (int i = 0; i < monthDays.Length; i++)
            {
                monthDays[i] = new DayNumbered
                {
                    Date = new DateOnly(year, month, i + 1),
                };
            }

            foreach (var holiday in holidays)
            {
                monthDays[holiday.Date.Day - 1].IsHoliday = true;
                monthDays[holiday.Date.Day - 1].HolidayName = holiday.LocalName;
            }

            return monthDays;
        }
    }
}

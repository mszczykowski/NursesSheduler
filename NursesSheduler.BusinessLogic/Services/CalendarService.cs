using NursesScheduler.BusinessLogic.Interfaces.Infrastructure;
using NursesScheduler.BusinessLogic.Interfaces.Services;
using NursesScheduler.Domain.Entities.Calendar;

namespace NursesScheduler.BusinessLogic.Services
{
    public sealed class CalendarService : ICalendarService
    {
        private readonly IScheduleConfigurationService _scheduleConfiguration;
        private readonly IHolidaysApiClient _holidaysApiClient;

        private ICollection<Holiday> _holidays;

        private int quarterIterator;

        public CalendarService(IHolidaysApiClient holidaysApiClient, IScheduleConfigurationService scheduleConfiguration)
        {
            _holidaysApiClient = holidaysApiClient;
            _scheduleConfiguration = scheduleConfiguration;
        }

        public async Task<Quarter> GetQuarter(int whichQuarter, int year)
        {
            Quarter quarter = new Quarter();

            if (_holidays != null && _holidays.Any() && _holidays.First().Date.Year != year)
            {
                _holidays = null;
            }

            int quarterStart = _scheduleConfiguration.GetQuarterStart();
            quarterIterator = 0;

            int month;

            for (int i = 0; i < 3; i++)
            {
                month = quarterStart + i + whichQuarter * 3;
                if (month > 12)
                {
                    month = 1;
                    year++;
                    _holidays = null;
                }
                quarter.Months[i] = await GetMonth(month, year);
                quarter.Months[i].MonthInQuarter = i + 1;
            }

            return quarter;
        }

        private async Task<Month> GetMonth(int monthNumber, int yearNumber)
        {
            if (_holidays == null) _holidays = await _holidaysApiClient.GetHolidays(yearNumber);

            List<Holiday> holidaysInRequestedMonth = _holidays.Where(h => h.Date.Month == monthNumber).ToList();

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

using NursesScheduler.BusinessLogic.Interfaces.Infrastructure;
using NursesScheduler.BusinessLogic.Interfaces.Services;
using NursesScheduler.Domain.Models.Calendar;

namespace NursesScheduler.BusinessLogic.Services
{
    public sealed class CalendarService : ICalendarService
    {
        private readonly IScheduleConfigurationService _scheduleConfiguration;
        private readonly IHolidaysApiClient _holidaysApiClient;

        private ICollection<Holiday> _holidays;

        public CalendarService(IHolidaysApiClient holidaysApiClient, IScheduleConfigurationService scheduleConfiguration)
        {
            _holidaysApiClient = holidaysApiClient;
            _scheduleConfiguration = scheduleConfiguration;
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
                    _holidays = null;
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
            if (_holidays == null || _holidays.Any() && _holidays.First().Date.Year != yearNumber) 
                _holidays = await _holidaysApiClient.GetHolidays(yearNumber);

            List<Holiday> holidaysInRequestedMonth = _holidays.Where(h => h.Date.Month == monthNumber).ToList();

            int daysInMonth = DateTime.DaysInMonth(yearNumber, monthNumber);

            Month month = new Month();
            month.MonthNumber = monthNumber;
            month.Year = yearNumber;

            month.Days = new Day[daysInMonth];

            holidaysInRequestedMonth.ForEach(holiday =>
            {
                month.Days[holiday.Date.Day - 1] = new Day(holiday.Date, holiday.Name);
            });

            for (int i = 0; i < daysInMonth; i++)
            {
                if (month.Days[i] == null)
                {
                    month.Days[i] = new Day(new DateTime(yearNumber, monthNumber, i + 1));
                }
            }

            return month;
        }
    }
}

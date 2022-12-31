using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NursesScheduler.BusinessLogic.Services
{
    internal class CalendarService
    {
        /*public async Task<Quarter> GetQuarter(int whichQuarter, int quarterStartYear)
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
        }*/
    }
}

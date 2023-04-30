using NursesScheduler.BusinessLogic.Abstractions.Managers;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.DomainModels;
using NursesScheduler.Domain.Models;

namespace NursesScheduler.BusinessLogic.Services
{
    internal class WorkTimeService : IWorkTimeService
    {
        private readonly IDepartamentSettingsManager _departamentSettingsManager;
        private readonly IHolidaysManager _hoidaysManager;

        public WorkTimeService(IDepartamentSettingsManager departamentSettingsManager, IHolidaysManager hoidaysManager)
        {
            _departamentSettingsManager = departamentSettingsManager;
            _hoidaysManager = hoidaysManager;
        }

        public async Task<TimeSpan> GetTotalWorkingHoursInMonth(int monthNumber, int yearNumber, int departamentId)
        {
            return await GetTotalWorkingHoursFromTo(new DateOnly(yearNumber, monthNumber, 1),
                new DateOnly(yearNumber, monthNumber, DateTime.DaysInMonth(yearNumber, monthNumber)), departamentId);
        }

        public async Task<TimeSpan> GetTotalWorkingHoursInQuarter(int quarterNumber, int yearNumber, int departamentId)
        {
            var departamentSettings = await _departamentSettingsManager.GetDepartamentSettings(departamentId);

            var workTimeInQuarter = TimeSpan.Zero;
            var quarterStart = departamentSettings.FirstQuarterStart;
            
            int monthNumber;

            for (int i = 0; i < 3; i++)
            {
                monthNumber = quarterStart + i + quarterNumber * 3;
                if (monthNumber > 12)
                {
                    monthNumber = 1;
                    yearNumber++;
                }

                workTimeInQuarter += await GetTotalWorkingHoursInMonth(monthNumber, yearNumber, departamentId);
            }

            return workTimeInQuarter;
        }

        public async Task<TimeSpan> GetTotalWorkingHoursFromTo(DateOnly from, DateOnly to, int departamentId)
        {
            var departamentSettings = await _departamentSettingsManager.GetDepartamentSettings(departamentId);

            var numberOfWorkingDays = await GetNumberOfWorkingDays(from, to);

            return numberOfWorkingDays * departamentSettings.WorkingTime;
        }

        public async Task<TimeSpan> GetTotalWorkingHoursFromDateArray(ICollection<DateOnly> dates, int departamentId)
        {
            var departamentSettings = await _departamentSettingsManager.GetDepartamentSettings(departamentId);

            var numberOfWorkingDays = 0;

            foreach(var date in dates)
            {
                if (IsWorkingDay(date, await _hoidaysManager.GetHolidays(date.Year)))
                    numberOfWorkingDays++;
            }

            return numberOfWorkingDays * departamentSettings.WorkingTime;
        }

        public async Task<int> GetQuarterNumber(int monthNumber, int departamentId)
        {
            var departamentSettings = await _departamentSettingsManager.GetDepartamentSettings(departamentId);

            var firstQuarterStart = departamentSettings.FirstQuarterStart;

            var relativeMonthNumber = 1;

            for (int i = firstQuarterStart; i != monthNumber; i = (i % 12) + 1)
            {
                relativeMonthNumber++;
            }

            return (int)(Math.Ceiling((decimal)(relativeMonthNumber) / 3));
        }

        public async Task<TimeSpan> GetSurplusWorkTime(int monthNumber, int yearNumber, int nurseCount, int departamentId)
        {
            var departamentSettings = await _departamentSettingsManager.GetDepartamentSettings(departamentId);

            var workingTimeInMonthPerNurse = await GetTotalWorkingHoursInMonth(monthNumber, yearNumber, departamentId);

            var totalNursesWorkTime = workingTimeInMonthPerNurse * nurseCount;
            var minimalTotalWorkTimeToAssign =
                departamentSettings.TargetNumberOfNursesOnShift * 2
                * TimeSpan.FromHours(12) 
                * DateTime.DaysInMonth(yearNumber, monthNumber);

            return totalNursesWorkTime - minimalTotalWorkTimeToAssign;
        }

        public TimeSpan GetWorkingTimeFromWorkDays(ICollection<NurseWorkDay> nurserWorkDays)
        {
            var workTime = TimeSpan.Zero;

            foreach(var workDay in nurserWorkDays)
            {
                workTime += workDay.ShiftEnd - workDay.ShiftStart;
            }

            return workTime;
        }

        private bool IsWorkingDay(DateOnly date, ICollection<Holiday> holidays)
        {
            return date.DayOfWeek != DayOfWeek.Sunday && date.DayOfWeek != DayOfWeek.Saturday
                && !holidays.Any(h => h.Date.Equals(date));
        }

        private async Task<int> GetNumberOfWorkingDays(DateOnly from, DateOnly to)
        {
            var holidays = await _hoidaysManager.GetHolidays(from.Year);

            var numberOfWorkingDays = 0;

            for (var date = from; date <= to; date = date.AddDays(1))
            {
                if (IsWorkingDay(date, holidays))
                    numberOfWorkingDays++;
            }
            return numberOfWorkingDays;
        }
    }
}

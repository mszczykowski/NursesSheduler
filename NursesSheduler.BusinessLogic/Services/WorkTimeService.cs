using NursesScheduler.BusinessLogic.Abstractions.Managers;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.Models.Calendar;

namespace NursesScheduler.BusinessLogic.Services
{
    internal class WorkTimeService : IWorkTimeService
    {
        private readonly IWorkTimeConfigurationManager _workTimeConfigurationManager;
        private readonly IHolidaysManager _hoidaysManager;

        public WorkTimeService(IWorkTimeConfigurationManager workTimeConfigurationManager, IHolidaysManager hoidaysManager)
        {
            _workTimeConfigurationManager = workTimeConfigurationManager;
            _hoidaysManager = hoidaysManager;
        }

        public async Task<TimeSpan> GetTotalWorkingHours(DateOnly from, DateOnly to)
        {
            var numberOfWorkingDays = await GetNumberOfWorkingDays(from, to);

            var workingDayLength = await _workTimeConfigurationManager.GetWorkDayLenght();

            return numberOfWorkingDays * workingDayLength;
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

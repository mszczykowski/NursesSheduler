using NursesScheduler.BusinessLogic.Abstractions.Managers;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.DomainModels;
using NursesScheduler.Domain.Enums;
using NursesScheduler.Domain.Models;

namespace NursesScheduler.BusinessLogic.Services
{
    internal sealed class WorkTimeService : IWorkTimeService
    {
        private readonly IHolidaysManager _hoidaysManager;

        private readonly static TimeSpan REGULAR_SHIFT_LENGHT = TimeSpan.FromHours(12);

        public WorkTimeService(IHolidaysManager hoidaysManager)
        {
            _hoidaysManager = hoidaysManager;
        }

        public async Task<TimeSpan> GetTotalWorkingHoursInMonth(int monthNumber, int yearNumber, 
            DepartamentSettings departamentSettings)
        {
            return await GetTotalWorkingHoursFromTo(new DateOnly(yearNumber, monthNumber, 1),
                new DateOnly(yearNumber, monthNumber, DateTime.DaysInMonth(yearNumber, monthNumber)), departamentSettings);
        }

        public async Task<TimeSpan> GetTotalWorkingHoursInQuarter(int quarterNumber, int yearNumber, 
            DepartamentSettings departamentSettings)
        {
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

                workTimeInQuarter += await GetTotalWorkingHoursInMonth(monthNumber, yearNumber, departamentSettings);
            }

            return workTimeInQuarter;
        }

        public async Task<TimeSpan> GetTotalWorkingHoursFromTo(DateOnly from, DateOnly to, 
            DepartamentSettings departamentSettings)
        {
            var numberOfWorkingDays = await GetNumberOfWorkingDays(from, to);

            return numberOfWorkingDays * departamentSettings.WorkingTime;
        }

        public async Task<TimeSpan> GetTotalWorkingHoursFromDateArray(ICollection<DateOnly> dates, 
            DepartamentSettings departamentSettings)
        {
            var numberOfWorkingDays = 0;

            foreach(var date in dates)
            {
                if (IsWorkingDay(date, await _hoidaysManager.GetHolidays(date.Year)))
                    numberOfWorkingDays++;
            }

            return numberOfWorkingDays * departamentSettings.WorkingTime;
        }

        public async Task<int> GetQuarterNumber(int monthNumber, DepartamentSettings departamentSettings)
        {
            var firstQuarterStart = departamentSettings.FirstQuarterStart;

            var relativeMonthNumber = 1;

            for (int i = firstQuarterStart; i != monthNumber; i = (i % 12) + 1)
            {
                relativeMonthNumber++;
            }

            return (int)(Math.Ceiling((decimal)(relativeMonthNumber) / 3));
        }

        public async Task<TimeSpan> GetSurplusWorkTime(int monthNumber, int yearNumber, int nurseCount, 
            DepartamentSettings departamentSettings)
        {
            var workingTimeInMonthPerNurse = await GetTotalWorkingHoursInMonth(monthNumber, yearNumber, departamentSettings);

            var totalNursesWorkTime = workingTimeInMonthPerNurse * nurseCount;
            var minimalTotalWorkTimeToAssign =
                departamentSettings.TargetNumberOfNursesOnShift * 2
                * REGULAR_SHIFT_LENGHT
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
                && !holidays.Any(h => DateOnly.FromDateTime(h.Date) == date);
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

        public async Task<TimeSpan> GetTimeForMorningShifts(int quarterNumber, int yearNumber, 
            DepartamentSettings departamentSettings)
        {
            var timeForMorningShifts = TimeSpan.Zero;
            int monthNumber;

            for (int i = 0; i < 3; i++)
            {
                monthNumber = departamentSettings.FirstQuarterStart + i + quarterNumber * 3;
                if(monthNumber > 12)
                {
                    monthNumber = 1;
                    yearNumber++;
                }
                
                var workTimeInMonth = await GetTotalWorkingHoursInMonth(monthNumber, yearNumber, departamentSettings);

                timeForMorningShifts = workTimeInMonth - (int)Math.Floor(workTimeInMonth / REGULAR_SHIFT_LENGHT)
                    * REGULAR_SHIFT_LENGHT;
            }

            return timeForMorningShifts;
        }

        public ICollection<MorningShift> CalculateMorningShifts(TimeSpan timeForMorningShifts, 
            DepartamentSettings departamentSettings)
        {
            var lengths = new List<TimeSpan>();

            if(timeForMorningShifts < departamentSettings.TargetMinimalMorningShiftLenght)
            {
                timeForMorningShifts += REGULAR_SHIFT_LENGHT;
            }

            while (timeForMorningShifts > REGULAR_SHIFT_LENGHT
                && timeForMorningShifts - REGULAR_SHIFT_LENGHT > departamentSettings.TargetMinimalMorningShiftLenght)
            {
                timeForMorningShifts -= REGULAR_SHIFT_LENGHT;
                lengths.Add(REGULAR_SHIFT_LENGHT);
            }

            if (timeForMorningShifts > REGULAR_SHIFT_LENGHT)
            {
                lengths.Add(timeForMorningShifts / 2);
                lengths.Add(timeForMorningShifts / 2);
            }
            else lengths.Add(timeForMorningShifts);

            var morningShifts = new MorningShift[3];

            for(int i = 0; i < 3; i++)
            {
                morningShifts[i] = new MorningShift
                {
                    Index = (MorningShiftIndex)i,
                    ShiftLength = TimeSpan.Zero,
                };
            }

            int j = 0;
            foreach(var length in lengths)
            {
                morningShifts[j++].ShiftLength = length;
            }

            return morningShifts;
        }
    }
}

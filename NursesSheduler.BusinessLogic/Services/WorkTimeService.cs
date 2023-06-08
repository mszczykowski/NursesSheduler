using NursesScheduler.BusinessLogic.Abstractions.CacheManagers;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.Services
{
    internal sealed class WorkTimeService : IWorkTimeService
    {
        private readonly IHolidaysManager _hoidaysManager;

        public WorkTimeService(IHolidaysManager hoidaysManager)
        {
            _hoidaysManager = hoidaysManager;
        }

        public TimeSpan GetWorkTimeFromDays(ICollection<Day> days, DepartamentSettings departamentSettings)
        {
            var workTime = TimeSpan.Zero;

            foreach(var day in days)
            {
                if(day.IsWorkingDay)
                {
                    workTime += departamentSettings.WorkingTime;
                }
            }

            return workTime;
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
                * GeneralConstants.RegularShiftLenght
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
        private bool IsWorkingDay(Day day)
        {
            return !day.IsHoliday && day.Date.DayOfWeek != DayOfWeek.Sunday && day.Date.DayOfWeek != DayOfWeek.Saturday;
        }

        public int GetNumberOfWorkingDays(Day[] days)
        {
            var result = 0;
            foreach(var day in days)
            {
                if(IsWorkingDay(day))
                    result++;
            }
            return result;
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

                timeForMorningShifts = workTimeInMonth - (int)Math.Floor(workTimeInMonth / GeneralConstants.RegularShiftLenght)
                    * GeneralConstants.RegularShiftLenght;
            }

            return timeForMorningShifts;
        }

        public ICollection<MorningShift> CalculateMorningShifts(TimeSpan timeForMorningShifts, 
            DepartamentSettings departamentSettings)
        {
            var lengths = new List<TimeSpan>();

            if(timeForMorningShifts < departamentSettings.TargetMinimalMorningShiftLenght)
            {
                timeForMorningShifts += GeneralConstants.RegularShiftLenght;
            }

            while (timeForMorningShifts > GeneralConstants.RegularShiftLenght
                && timeForMorningShifts - GeneralConstants.RegularShiftLenght > departamentSettings.TargetMinimalMorningShiftLenght)
            {
                timeForMorningShifts -= GeneralConstants.RegularShiftLenght;
                lengths.Add(GeneralConstants.RegularShiftLenght);
            }

            if (timeForMorningShifts > GeneralConstants.RegularShiftLenght)
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

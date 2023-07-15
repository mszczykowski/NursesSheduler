﻿using NursesScheduler.BusinessLogic.Abstractions.Infrastructure.Providers;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Enums;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Services
{
    internal sealed class WorkTimeService : IWorkTimeService
    {
        private readonly ICalendarService _calendarService;

        public WorkTimeService(ICalendarService calendarService)
        {
            _calendarService = calendarService;
        }

        public TimeSpan GetWorkTimeFromDays(IEnumerable<Day> days, TimeSpan regularDayWorkTime)
        {
            var workTime = TimeSpan.Zero;

            foreach (var day in days)
            {
                if (day.IsWorkingDay)
                {
                    workTime += regularDayWorkTime;
                }
            }

            return workTime;
        }

        public async Task<TimeSpan> GetTotalWorkingHoursInMonth(int monthNumber, int yearNumber,
            TimeSpan regularDayWorkTime)
        {
            var month = await _calendarService.GetMonthDaysAsync(monthNumber, yearNumber);

            return GetWorkTimeFromDays(month, regularDayWorkTime);

        }

        public async Task<TimeSpan> GetTotalWorkingHoursInQuarter(int quarterNumber, int yearNumber,
            DepartamentSettings departamentSettings)
        {
            var workTimeInQuarter = TimeSpan.Zero;

            var quarterDates = _calendarService.GetMonthsInQuarterDatesAsync(departamentSettings.FirstQuarterStart, quarterNumber,
                yearNumber);

            foreach (var date in quarterDates)
            {
                workTimeInQuarter += await GetTotalWorkingHoursInMonth(date.Month, date.Year,
                    departamentSettings.WorkDayLength);
            }

            return workTimeInQuarter;
        }

        public async Task<TimeSpan> GetSurplusWorkTime(int monthNumber, int yearNumber, int nurseCount,
            DepartamentSettings departamentSettings)
        {
            var WorkDayLengthInMonthPerNurse = await GetTotalWorkingHoursInMonth(monthNumber, yearNumber,
                departamentSettings.WorkDayLength);

            var totalNursesWorkTime = WorkDayLengthInMonthPerNurse * nurseCount;
            var minimalTotalWorkTimeToAssign =
                departamentSettings.TargetNumberOfNursesOnShift * 2
                * GeneralConstants.RegularShiftLenght
                * DateTime.DaysInMonth(yearNumber, monthNumber);

            return totalNursesWorkTime - minimalTotalWorkTimeToAssign;
        }

        public TimeSpan GetWorkDayLengthFromWorkDays(IEnumerable<NurseWorkDay> nurserWorkDays)
        {
            var workTime = TimeSpan.Zero;

            foreach (var workDay in nurserWorkDays)
            {
                if (workDay.MorningShift != null)
                {
                    workTime += workDay.MorningShift.ShiftLength;
                }
                else if (workDay.ShiftType != ShiftTypes.None)
                {
                    workTime += GeneralConstants.RegularShiftLenght;
                }
            }

            return workTime;
        }

        public async Task<TimeSpan> GetTimeForMorningShifts(int quarterNumber, int yearNumber,
            DepartamentSettings departamentSettings)
        {
            var timeForMorningShifts = TimeSpan.Zero;

            var monthsInQuarter = _calendarService.GetMonthsInQuarterDatesAsync(departamentSettings.FirstQuarterStart,
                quarterNumber, yearNumber);

            foreach(var monthDate in monthsInQuarter)
            {
                var workTimeInMonth = await GetTotalWorkingHoursInMonth(monthDate.Month, monthDate.Year,
                    departamentSettings.WorkDayLength);

                timeForMorningShifts = workTimeInMonth - (int)Math.Floor(workTimeInMonth /
                    GeneralConstants.RegularShiftLenght) * GeneralConstants.RegularShiftLenght;
            }

            return timeForMorningShifts;
        }

        public ICollection<MorningShift> CalculateMorningShifts(TimeSpan timeForMorningShifts,
            DepartamentSettings departamentSettings)
        {
            var lengths = new List<TimeSpan>();

            if (timeForMorningShifts < departamentSettings.TargetMinimalMorningShiftLenght)
            {
                timeForMorningShifts += GeneralConstants.RegularShiftLenght;
            }

            while (timeForMorningShifts > GeneralConstants.RegularShiftLenght
                && timeForMorningShifts - GeneralConstants.RegularShiftLenght
                > departamentSettings.TargetMinimalMorningShiftLenght)
            {
                timeForMorningShifts -= GeneralConstants.RegularShiftLenght;
                lengths.Add(GeneralConstants.RegularShiftLenght);
            }

            if (timeForMorningShifts > GeneralConstants.RegularShiftLenght)
            {
                lengths.Add(timeForMorningShifts / 2);
                lengths.Add(timeForMorningShifts / 2);
            }
            else
            {
                lengths.Add(timeForMorningShifts);
            }

            var morningShifts = new MorningShift[3];

            for (int i = 0; i < 3; i++)
            {
                morningShifts[i] = new MorningShift
                {
                    Index = (MorningShiftIndex)i,
                    ShiftLength = TimeSpan.Zero,
                };
            }

            int j = 0;
            foreach (var length in lengths)
            {
                morningShifts[j++].ShiftLength = length;
            }

            return morningShifts;
        }
    }
}

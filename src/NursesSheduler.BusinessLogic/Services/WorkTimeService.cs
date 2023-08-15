using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.Constants;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Enums;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Services
{
    internal sealed class WorkTimeService : IWorkTimeService
    {
        public IEnumerable<MorningShift> CalculateMorningShifts(TimeSpan timeForMorningShifts,
            DepartamentSettings departamentSettings)
        {
            var lengths = new List<TimeSpan>();

            if (timeForMorningShifts < departamentSettings.TargetMinimalMorningShiftLenght)
            {
                timeForMorningShifts += ScheduleConstatns.RegularShiftLength;
            }

            while (timeForMorningShifts > ScheduleConstatns.RegularShiftLength
                && timeForMorningShifts - ScheduleConstatns.RegularShiftLength
                > departamentSettings.TargetMinimalMorningShiftLenght)
            {
                timeForMorningShifts -= ScheduleConstatns.RegularShiftLength;
                lengths.Add(ScheduleConstatns.RegularShiftLength);
            }

            if (timeForMorningShifts > ScheduleConstatns.RegularShiftLength)
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

        public TimeSpan GetHoursFromLastAssignedShift(int toDay, IEnumerable<NurseWorkDay> nurseWorkDays)
        {
            TimeSpan hoursFromLastAssignedShift = TimeSpan.Zero;
            foreach (var workDay in nurseWorkDays.Where(d => d.Day < toDay).OrderByDescending(d => d.Day))
            {
                switch (workDay.ShiftType)
                {
                    case ShiftTypes.None:
                        hoursFromLastAssignedShift += TimeSpan.FromDays(1);
                        break;
                    case ShiftTypes.Day:
                        hoursFromLastAssignedShift += ScheduleConstatns.RegularShiftLength;
                        return hoursFromLastAssignedShift;
                    case ShiftTypes.Night:
                        return hoursFromLastAssignedShift;
                    case ShiftTypes.Morning:
                        var morningShiftLenght = workDay.MorningShift?.ShiftLength
                            ?? throw new InvalidOperationException("GetHoursFromLastShift: Morning shift length cannot be null");

                        hoursFromLastAssignedShift += 2 * ScheduleConstatns.RegularShiftLength - morningShiftLenght;
                        return hoursFromLastAssignedShift;
                }
            }
            return hoursFromLastAssignedShift;
        }
        public TimeSpan GetHoursFromLastAssignedShift(IEnumerable<NurseWorkDay> nurseWorkDays)
        {
            return GetHoursFromLastAssignedShift(31, nurseWorkDays);
        }
        public TimeSpan GetHoursToFirstAssignedShift(int fromDay, IEnumerable<NurseWorkDay> nurseWorkDays)
        {
            TimeSpan hoursToFirstAssignedShift = TimeSpan.Zero;
            foreach (var workDay in nurseWorkDays.Where(wd => wd.Day >= fromDay).OrderBy(wd => wd.Day))
            {
                switch (workDay.ShiftType)
                {
                    case ShiftTypes.None:
                        hoursToFirstAssignedShift += TimeSpan.FromDays(1);
                        break;
                    case ShiftTypes.Day:
                        return hoursToFirstAssignedShift;
                    case ShiftTypes.Night:
                        hoursToFirstAssignedShift += ScheduleConstatns.RegularShiftLength;
                        return hoursToFirstAssignedShift;
                    case ShiftTypes.Morning:
                        return hoursToFirstAssignedShift;
                }
            }
            return hoursToFirstAssignedShift;
        }

        public TimeSpan GetHoursToFirstAssignedShift(IEnumerable<NurseWorkDay> nurseWorkDays)
        {
            return GetHoursToFirstAssignedShift(1, nurseWorkDays);
        }

        public TimeSpan GetTimeOffTimeToAssign(IEnumerable<NurseWorkDay> nurseWorkDays,
            IEnumerable<Day> monthDays, DepartamentSettings departamentSettings)
        {
            return nurseWorkDays.Where(wd => wd.IsTimeOff && monthDays.First(d => d.Date.Day == wd.Day).IsWorkDay)
                .Count() * departamentSettings.WorkDayLength;
        }

        public TimeSpan GetAssignedShiftWorkTime(ShiftTypes shiftType, TimeSpan? morningShiftLenght)
        {
            switch (shiftType)
            {
                case ShiftTypes.None:
                    return TimeSpan.Zero;
                case ShiftTypes.Morning:
                    return morningShiftLenght ?? 
                        throw new InvalidOperationException("GetAssignedDayWorkTime: Morning shift length cannot be null");
                default:
                    return ScheduleConstatns.RegularShiftLength;
            }
        }

        public TimeSpan GetMonthWorkTimeBalance(int numberOfNurses, IEnumerable<Day> monthDays, 
            DepartamentSettings departamentSettings)
        {
            return (GetWorkTimeFromDays(monthDays, departamentSettings) * numberOfNurses)
                -
                (monthDays.Count() * departamentSettings.TargetMinNumberOfNursesOnShift * ScheduleConstatns.RegularShiftLength);
        }

        public TimeSpan GetWorkTimeFromDays(IEnumerable<Day> days, DepartamentSettings departamentSettings)
        {
            return days.Where(d => d.IsWorkDay).Count() * departamentSettings.WorkDayLength;
        }

        public TimeSpan GetShiftNightHours(ShiftTypes shiftType, Day day, DepartamentSettings departamentSettings)
        {
            if(shiftType != ShiftTypes.Night)
            {
                return TimeSpan.Zero;
            }

            if(day.IsWorkDay)
            {
                return ScheduleConstatns.RegularShiftLength;
            }

            return ScheduleConstatns.RegularShiftLength - departamentSettings.NightShiftHolidayEligibleHours;
        }

        public TimeSpan GetShiftHolidayHours(ShiftTypes shiftType, TimeSpan? optionalMorningShiftLenght, Day day, 
            DepartamentSettings departamentSettings)
        {
            if(day.IsWorkDay || shiftType == ShiftTypes.None)
            {
                return TimeSpan.Zero;
            }

            switch (shiftType)
            {
                case ShiftTypes.Day:
                    return departamentSettings.DayShiftHolidayEligibleHours;
                case ShiftTypes.Night:
                    return departamentSettings.NightShiftHolidayEligibleHours;
                case ShiftTypes.Morning:
                    var morningShiftLenght = optionalMorningShiftLenght
                         ?? throw new InvalidOperationException("GetShiftHolidayHours: Morning shift length cannot be null");

                    return morningShiftLenght > departamentSettings.DayShiftHolidayEligibleHours 
                        ? departamentSettings.DayShiftHolidayEligibleHours : morningShiftLenght;
                default:
                    return TimeSpan.Zero;
            }
        }
    }
}

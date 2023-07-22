using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.Constants;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Enums;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Services
{
    internal sealed class WorkTimeService : IWorkTimeService
    {
        public TimeSpan GetHoursFromLastAssignedShift(IEnumerable<NurseWorkDay> nurseWorkDays)
        {
            TimeSpan hoursFromLastAssignedShift = TimeSpan.Zero;
            foreach (var workDay in nurseWorkDays.OrderByDescending(d => d.Day))
            {
                switch (workDay.ShiftType)
                {
                    case ShiftTypes.None:
                        hoursFromLastAssignedShift += TimeSpan.FromDays(1);
                        break;
                    case ShiftTypes.Day:
                        hoursFromLastAssignedShift += ScheduleConstatns.RegularShiftLenght;
                        return hoursFromLastAssignedShift;
                    case ShiftTypes.Night:
                        return hoursFromLastAssignedShift;
                    case ShiftTypes.Morning:
                        var morningShiftLenght = workDay.MorningShift?.ShiftLength
                            ?? throw new InvalidOperationException("GetHoursFromLastShift: Morning shift length cannot be null");

                        hoursFromLastAssignedShift += 2 * ScheduleConstatns.RegularShiftLenght - morningShiftLenght;
                        return hoursFromLastAssignedShift;
                }
            }
            return hoursFromLastAssignedShift;
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
                        hoursToFirstAssignedShift += ScheduleConstatns.RegularShiftLenght;
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
                    return ScheduleConstatns.RegularShiftLenght;
            }
        }

        public TimeSpan GetMonthWorkTimeBalance(int numberOfNurses, IEnumerable<Day> monthDays, 
            DepartamentSettings departamentSettings)
        {
            return (GetWorkTimeFromDays(monthDays, departamentSettings) * numberOfNurses)
                -
                (monthDays.Count() * departamentSettings.TargetMinNumberOfNursesOnShift * ScheduleConstatns.RegularShiftLenght);
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
                return ScheduleConstatns.RegularShiftLenght;
            }

            return ScheduleConstatns.RegularShiftLenght - departamentSettings.NightShiftHolidayEligibleHours;
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

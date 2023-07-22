using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Enums;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Abstractions.Services
{
    internal interface IWorkTimeService
    {
        TimeSpan GetHoursFromLastAssignedShift(IEnumerable<NurseWorkDay> nurseWorkDays);
        TimeSpan GetHoursToFirstAssignedShift(int fromDay, IEnumerable<NurseWorkDay> nurseWorkDays);
        TimeSpan GetHoursToFirstAssignedShift(IEnumerable<NurseWorkDay> nurseWorkDays);
        TimeSpan GetTimeOffTimeToAssign(IEnumerable<NurseWorkDay> nurseWorkDays,
            IEnumerable<Day> monthDays, DepartamentSettings departamentSettings);
        TimeSpan GetAssignedShiftWorkTime(ShiftTypes shiftType, TimeSpan? morningShiftLenght);
        TimeSpan GetMonthWorkTimeBalance(int numberOfNurses, IEnumerable<Day> monthDays,
            DepartamentSettings departamentSettings);
        TimeSpan GetWorkTimeFromDays(IEnumerable<Day> days, DepartamentSettings departamentSettings);
        TimeSpan GetShiftNightHours(ShiftTypes shiftType, Day day, DepartamentSettings departamentSettings);
        TimeSpan GetShiftHolidayHours(ShiftTypes shiftType, TimeSpan? optionalMorningShiftLenght, Day day,
            DepartamentSettings departamentSettings);
    }
}

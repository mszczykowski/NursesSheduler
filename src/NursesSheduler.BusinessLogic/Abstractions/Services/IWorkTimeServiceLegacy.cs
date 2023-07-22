using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Abstractions.Services
{
    internal interface IWorkTimeServiceLegacy
    {
        ICollection<MorningShift> CalculateMorningShifts(TimeSpan timeForMorningShifts,
            DepartamentSettings departamentSettings);
        Task<TimeSpan> GetSurplusWorkTime(int monthNumber, int yearNumber, int nurseCount, 
            DepartamentSettings departamentSettings);
        Task<TimeSpan> GetTimeForMorningShifts(int quarterNumber, int yearNumber, 
            DepartamentSettings departamentSettings);
        Task<TimeSpan> GetTotalWorkingHoursInMonth(int monthNumber, int yearNumber, TimeSpan regularDayWorkTime);
        Task<TimeSpan> GetTotalWorkingHoursInQuarter(int quarterNumber, int yearNumber, 
            DepartamentSettings departamentSettings);
        TimeSpan GetWorkDayLengthFromWorkDays(IEnumerable<NurseWorkDay> nurserWorkDays);
        TimeSpan GetWorkTimeFromDays(IEnumerable<DayNumbered> days, TimeSpan regularDayWorkTime);
    }
}
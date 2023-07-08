using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Abstractions.Services
{
    internal interface IWorkTimeService
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
        TimeSpan GetWorkingTimeFromWorkDays(ICollection<NurseWorkDay> nurserWorkDays);
        TimeSpan GetWorkTimeFromDays(ICollection<Day> days, TimeSpan regularDayWorkTime);
    }
}
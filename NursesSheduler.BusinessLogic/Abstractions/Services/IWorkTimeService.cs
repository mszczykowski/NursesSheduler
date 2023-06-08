using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Abstractions.Services
{
    internal interface IWorkTimeService
    {
        Task<TimeSpan> GetTotalWorkingHoursInQuarter(int quarterNumber, int yearNumber, 
            DepartamentSettings departamentSettings);
        Task<TimeSpan> GetTotalWorkingHoursInMonth(int monthNumber, int yearNumber, 
            DepartamentSettings departamentSettings);
        Task<TimeSpan> GetTotalWorkingHoursFromTo(DateOnly from, DateOnly to, DepartamentSettings departamentSettings);
        Task<TimeSpan> GetTotalWorkingHoursFromDateArray(ICollection<DateOnly> dates, 
            DepartamentSettings departamentSettings);
        Task<TimeSpan> GetSurplusWorkTime(int monthNumber, int yearNumber, int nurseCount, 
            DepartamentSettings departamentSettings);
        Task<int> GetQuarterNumber(int monthNumber, DepartamentSettings departamentSettings);
        TimeSpan GetWorkingTimeFromWorkDays(ICollection<NurseWorkDay> nurserWorkDays);
        Task<TimeSpan> GetTimeForMorningShifts(int quarterNumber, int yearNumber,
            DepartamentSettings departamentSettings);
        ICollection<MorningShift> CalculateMorningShifts(TimeSpan timeForMorningShifts,
            DepartamentSettings departamentSettings);
        int GetNumberOfWorkingDays(Day[] days);
        TimeSpan GetWorkTimeFromDays(ICollection<Day> days, DepartamentSettings departamentSettings);
    }
}
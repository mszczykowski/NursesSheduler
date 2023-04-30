using NursesScheduler.Domain.DomainModels;

namespace NursesScheduler.BusinessLogic.Abstractions.Services
{
    internal interface IWorkTimeService
    {
        Task<TimeSpan> GetTotalWorkingHoursInQuarter(int quarterNumber, int yearNumber, int departamentId);
        Task<TimeSpan> GetTotalWorkingHoursInMonth(int monthNumber, int yearNumber, int departamentId);
        Task<TimeSpan> GetTotalWorkingHoursFromTo(DateOnly from, DateOnly to, int departamentId);
        Task<TimeSpan> GetTotalWorkingHoursFromDateArray(ICollection<DateOnly> dates, int departamentId);
        Task<TimeSpan> GetSurplusWorkTime(int monthNumber, int yearNumber, int nurseCount, int departamentId);
        Task<int> GetQuarterNumber(int monthNumber, int departamentId);
        TimeSpan GetWorkingTimeFromWorkDays(ICollection<NurseWorkDay> nurserWorkDays);
    }
}
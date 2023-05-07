using NursesScheduler.Domain.DomainModels;

namespace NursesScheduler.BusinessLogic.Abstractions.Services
{
    internal interface ISchedulesService
    {
        Task<Schedule> GetNewSchedule(int monthNumber, int yearNumber, int departamentId,
            DepartamentSettings departamentSettings);
        Task SetTimeOffs(Schedule schedule, DepartamentSettings departamentSettings);
        Task SetNurseWorkTimes(Schedule schedule);
    }
}
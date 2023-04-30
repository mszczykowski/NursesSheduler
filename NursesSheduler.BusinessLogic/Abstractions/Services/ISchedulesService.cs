using NursesScheduler.Domain.DomainModels;

namespace NursesScheduler.BusinessLogic.Abstractions.Services
{
    internal interface ISchedulesService
    {
        Task<Schedule> GetNewSchedule(int monthNumber, int yearNumber, int departamentId);
        Task SetTimeOffs(Schedule schedule);
        Task SetNurseWorkTimes(Schedule schedule);
    }
}
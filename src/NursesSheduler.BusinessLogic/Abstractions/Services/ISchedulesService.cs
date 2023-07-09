using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Abstractions.Services
{
    internal interface ISchedulesService
    {
        Task<Schedule> GetNewSchedule(int monthNumber, int yearNumber, int departamentId,
            DepartamentSettings departamentSettings);
        Task SetTimeOffs(Schedule schedule, DepartamentSettings departamentSettings);
        Task CalculateNurseWorkTimes(Schedule schedule);
        Task<Schedule?> GetPreviousSchedule(Schedule currentSchedule);
        Task UpdateScheduleNurses(Schedule schedule);
    }
}
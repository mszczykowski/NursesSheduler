using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Abstractions.Services
{
    internal interface ISchedulesService
    {
        Task<Schedule> CreateNewScheduleAsync(int month, Quarter quarter);
        Task SetTimeOffsAsync(int year, int month, ScheduleNurse scheduleNurse);
    }
}
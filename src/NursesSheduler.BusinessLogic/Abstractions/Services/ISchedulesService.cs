using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Abstractions.Services
{
    internal interface ISchedulesService
    {
        void ResolveMorningShifts(Schedule schedule, IEnumerable<MorningShift> morningShifts);
        Task<Schedule> CreateNewScheduleAsync(int month, Quarter quarter);
        Task SetTimeOffsAsync(int year, int month, ScheduleNurse scheduleNurse);
        Task<int> UpsertSchedule(Schedule updatedSchdeule, CancellationToken cancellationToken);
    }
}
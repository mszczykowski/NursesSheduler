using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.Abstractions.Services
{
    internal interface ISchedulesService
    {
        void ResolveMorningShifts(Schedule schedule, IEnumerable<MorningShift> morningShifts);
        Task<Schedule> CreateNewScheduleAsync(int month, Quarter quarter);
        Task SetTimeOffsAsync(int year, int month, Schedule schedule);
        Task<int> UpsertSchedule(Schedule updatedSchdeule, CancellationToken cancellationToken);
        void SetScheduleStats(Schedule schedule, ScheduleStats scheduleStats);
    }
}
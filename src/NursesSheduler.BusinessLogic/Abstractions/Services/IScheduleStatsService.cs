using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.Abstractions.Services
{
    public interface IScheduleStatsService
    {
        Task<ScheduleStats> GetScheduleStatsAsync(int year, int month, int departamentId);
        Task<ScheduleStats> GetScheduleStatsAsync(int year, int departamentId, Schedule schedule);
        Task<NurseScheduleStats> RecalculateNurseScheduleStats(int year, int month, int departamentId, 
            ScheduleNurse scheduleNurse);
    }
}
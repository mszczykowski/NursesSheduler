using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.Abstractions.Services
{
    public interface IScheduleStatsService
    {
        Task<NurseScheduleStats> RecalculateNurseScheduleStats(ScheduleNurse scheduleNurse,
            int departamentId, int year, int month);
        
        Task<ScheduleStats> GetScheduleStatsAsync(int year, int month, int departamentId);
        Task<ScheduleStats> GetScheduleStatsAsync(Schedule schedule, int departamentId, int year);
    }
}
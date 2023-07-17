using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.Abstractions.Services
{
    public interface IStatsService
    {
        Task<NurseScheduleStats> RecalculateNurseScheduleStats(ScheduleNurse scheduleNurse,
            int departamentId, int year, int month);
        Task<NurseStats> RecalculateQuarterNurseStatsAsync(NurseStats currentScheduleNursesStats, int year, int month, 
            int departamentId);
        Task<QuarterStats> GetQuarterStatsAsync(ScheduleStats currentScheduleStats, int year, int month, int departamentId);
        Task<ScheduleStats> GetScheduleStatsAsync(int year, int month, int departamentId);
        Task<ScheduleStats> GetScheduleStatsAsync(Schedule schedule, int departamentId, int year);
    }
}
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.Abstractions.Services
{
    public interface IStatsService
    {
        Task<IEnumerable<NurseScheduleStats>> GetNurseScheduleStats(Schedule schedule, int departamentId, int year);
        Task<IEnumerable<NurseStats>> GetQuarterNurseStats(ScheduleStats currentScheduleStats, int year, int month,
            int departamentId);
        Task<QuarterStats> GetQuarterStats(ScheduleStats currentScheduleStats, int year, int month, int departamentId);
        Task<ScheduleStats> GetScheduleStatsAsync(int year, int month, int departamentId);
        Task<ScheduleStats> GetScheduleStatsAsync(Schedule schedule, int departamentId, int year);
    }
}
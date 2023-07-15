using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.Abstractions.Services
{
    public interface IStatsService
    {
        IEnumerable<NurseStats> GetQuarterNurseStats(IEnumerable<ScheduleStats> quarterScheduleStats);
        QuarterStats GetQuarterStats(IEnumerable<ScheduleStats> quarterScheduleStats);
        Task<ScheduleStats> GetScheduleStatsAsync(int year, int month, int departamentId);
        Task<ScheduleStats> GetScheduleStatsAsync(Schedule schedule, int departamentId, int year);
    }
}
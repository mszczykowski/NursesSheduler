using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.Abstractions.Services
{
    public interface IQuarterStatsService
    {
        Task<NurseStats> RecalculateQuarterNurseStatsAsync(NurseStats currentScheduleNursesStats, int year, int month,
            int departamentId);
        Task<QuarterStats> GetQuarterStatsAsync(ScheduleStats currentScheduleStats, int year, int month, int departamentId);
    }
}

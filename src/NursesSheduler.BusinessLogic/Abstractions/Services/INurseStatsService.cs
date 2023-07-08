using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Abstractions.Services
{
    internal interface INurseStatsService
    {
        Task<ICollection<NurseQuarterStats>> GetNurseQuarterStats(Schedule currentSchedule,
            DepartamentSettings departamentSettings)
    }
}

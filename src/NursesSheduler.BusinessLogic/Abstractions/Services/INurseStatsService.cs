using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Abstractions.Services
{
    internal interface INurseStatsService
    {
        Task<ICollection<NurseQuarterStats>> GetNurseQuarterStats(Schedule currentSchedule,
            DepartamentSettings departamentSettings);
    }
}

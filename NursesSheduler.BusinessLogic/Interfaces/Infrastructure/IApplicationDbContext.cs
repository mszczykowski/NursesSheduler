using Microsoft.EntityFrameworkCore;
using NursesScheduler.Domain.DatabaseModels;
using NursesScheduler.Domain.DatabaseModels.Schedules;

namespace NursesScheduler.BusinessLogic.Interfaces.Infrastructure
{
    public interface IApplicationDbContext
    {
        DbSet<Nurse> Nurses { get; }
        DbSet<Departament> Departaments { get; }
        DbSet<Schedule> Schedules { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}

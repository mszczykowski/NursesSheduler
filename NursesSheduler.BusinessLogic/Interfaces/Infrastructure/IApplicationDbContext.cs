using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NursesScheduler.Domain.DatabaseModels;
using NursesScheduler.Domain.DatabaseModels.Schedules;

namespace NursesScheduler.BusinessLogic.Interfaces.Infrastructure
{
    public interface IApplicationDbContext
    {
        DbSet<Nurse> Nurses { get; }
        DbSet<Departament> Departaments { get; }
        DbSet<Schedule> Schedules { get; }
        DbSet<Shift> Shifts { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        EntityEntry Entry(object entity);
    }
}

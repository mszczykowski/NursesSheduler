using Microsoft.EntityFrameworkCore;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.Persistance.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Nurse> Nurses { get; set; }
        DbSet<Departament> Departaments { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}

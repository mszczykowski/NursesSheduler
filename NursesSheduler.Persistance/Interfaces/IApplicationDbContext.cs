using Microsoft.EntityFrameworkCore;
using NursesSheduler.Domain.Entities;

namespace NursesSheduler.Persistance.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Nurse> Nurses { get; set; }
        DbSet<Departament> Departaments { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}

using Microsoft.EntityFrameworkCore;
using NursesSheduler.Domain.Entities;
using NursesSheduler.Persistance.Configuration;
using NursesSheduler.Persistance.Interfaces;

namespace NursesSheduler.Persistance.Context
{
    internal class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<Nurse> Nurses { get; set; }
        public DbSet<Departament> Departaments { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(NurseConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DepartamentConfiguration).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Interfaces.Infrastructure;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Infrastructure.Configuration;

namespace NursesScheduler.Infrastructure.Context
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
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

using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Infrastructure.Configuration;

namespace NursesScheduler.Infrastructure.Context
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<Absence> Absences { get; set; }
        public DbSet<AbsencesSummary> AbsencesSummaries { get; set; }
        public DbSet<Departament> Departaments { get; set; }
        public DbSet<DepartamentSettings> DepartamentSettings { get; set; }
        public DbSet<MorningShift> MorningShifts { get; set; }
        public DbSet<Nurse> Nurses { get; set; }
        public DbSet<NurseWorkDay> NursesWorkDays { get; set; }
        public DbSet<Quarter> Quarters { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<ScheduleNurse> ScheduleNurses { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(NurseConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DepartamentConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ScheduleConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Absence).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}

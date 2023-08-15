using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.Infrastructure.Configuration
{
    internal sealed class NurseConfiguration : IEntityTypeConfiguration<Nurse>
    {
        public void Configure(EntityTypeBuilder<Nurse> builder)
        {
            builder.HasKey(n => n.NurseId);

            builder.HasMany(n => n.ScheduleNurses)
                .WithOne(sn => sn.Nurse)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(n => n.AbsencesSummaries)
                .WithOne(s => s.Nurse)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(n => n.Name)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(n => n.Surname)
                .IsRequired()
                .HasMaxLength(20);
        }
    }
}

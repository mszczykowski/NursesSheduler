using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.Infrastructure.Configuration
{
    internal sealed class DepartamentConfiguration : IEntityTypeConfiguration<Departament>
    {
        public void Configure(EntityTypeBuilder<Departament> builder)
        {
            builder.HasKey(d => d.DepartamentId);

            builder.HasMany(d => d.Nurses)
                .WithOne(n => n.Departament)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(d => d.Quarters)
                .WithOne(q => q.Departament)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.DepartamentSettings)
                .WithOne(s => s.Departament)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(40);
        }
    }
}

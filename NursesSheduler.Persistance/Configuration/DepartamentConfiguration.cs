using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NursesScheduler.Domain.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NursesScheduler.Infrastructure.Configuration
{
    internal class DepartamentConfiguration : IEntityTypeConfiguration<Departament>
    {
        public void Configure(EntityTypeBuilder<Departament> builder)
        {
            builder.HasKey(d => d.DepartamentId);

            builder.HasMany(d => d.Nurses)
                .WithOne(n => n.Departament)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(40);
        }
    }
}

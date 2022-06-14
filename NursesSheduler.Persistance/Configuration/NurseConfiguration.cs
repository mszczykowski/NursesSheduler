using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NursesScheduler.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NursesScheduler.Persistance.Configuration
{
    internal class NurseConfiguration : IEntityTypeConfiguration<Nurse>
    {
        public void Configure(EntityTypeBuilder<Nurse> builder)
        {
            builder.HasKey(n => n.Id);

            builder.Property(n => n.Name)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(n => n.Surname)
                .IsRequired()
                .HasMaxLength(30);
        }
    }
}

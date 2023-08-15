using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.Infrastructure.Configuration
{
    internal sealed class DepartamentSettingsConfiguration : IEntityTypeConfiguration<DepartamentSettings>
    {
        public void Configure(EntityTypeBuilder<DepartamentSettings> builder)
        {
            builder.HasKey(s => s.DepartamentSettingsId);

            //builder.HasOne(s => s.Departament)
            //    .WithOne(d => d.DepartamentSettings);
        }
    }
}

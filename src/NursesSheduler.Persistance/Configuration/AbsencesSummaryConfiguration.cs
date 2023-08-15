using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.Infrastructure.Configuration
{
    internal sealed class AbsencesSummaryConfiguration : IEntityTypeConfiguration<AbsencesSummary>
    {
        public void Configure(EntityTypeBuilder<AbsencesSummary> builder)
        {
            builder.HasKey(s => s.AbsencesSummaryId);

            builder.HasMany(s => s.Absences)
                .WithOne(a => a.AbsencesSummary)
                .OnDelete(DeleteBehavior.Cascade);

            //builder.HasOne(s => s.Nurse)
            //    .WithMany(n => n.AbsencesSummaries)
            //    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

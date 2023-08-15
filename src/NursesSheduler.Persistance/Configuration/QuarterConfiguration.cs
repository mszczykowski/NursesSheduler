using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.Infrastructure.Configuration
{
    internal sealed class QuarterConfiguration : IEntityTypeConfiguration<Quarter>
    {
        public void Configure(EntityTypeBuilder<Quarter> builder)
        {
            builder.HasKey(q => q.QuarterId);

            builder.HasMany(q => q.Schedules)
                .WithOne(s => s.Quarter)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(q => q.MorningShifts)
                .WithOne(m => m.Quarter)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

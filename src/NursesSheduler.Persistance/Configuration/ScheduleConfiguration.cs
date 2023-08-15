using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.Infrastructure.Configuration
{
    internal sealed class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            builder.HasKey(s => s.ScheduleId);

            builder.HasMany(s => s.ScheduleNurses)
                .WithOne(sn => sn.Schedule)
                .OnDelete(DeleteBehavior.Cascade);

            //builder.HasOne(s => s.Quarter)
            //    .WithMany(q => q.Schedules)
            //    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.Infrastructure.Configuration
{
    internal sealed class ScheduleNurseConfiguration : IEntityTypeConfiguration<ScheduleNurse>
    {
        public void Configure(EntityTypeBuilder<ScheduleNurse> builder)
        {
            builder.HasKey(sn => sn.ScheduleNurseId);

            builder.HasMany(sn => sn.NurseWorkDays)
                .WithOne(wd => wd.ScheduleNurse)
                .OnDelete(DeleteBehavior.Cascade);

            //builder.HasOne(sn => sn.Nurse)
            //    .WithMany(n => n.ScheduleNurses)
            //    .OnDelete(DeleteBehavior.NoAction);

            //builder.HasOne(sn => sn.Schedule)
            //    .WithMany(s => s.ScheduleNurses)
            //    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

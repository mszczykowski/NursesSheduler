using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.Infrastructure.Configuration
{
    internal sealed class NurseWorkDayConfiguration : IEntityTypeConfiguration<NurseWorkDay>
    {
        public void Configure(EntityTypeBuilder<NurseWorkDay> builder)
        {
            builder.HasKey(wd => wd.NurseWorkDayId);

            //builder.HasOne(wd => wd.ScheduleNurse)
            //    .WithMany(sn => sn.NurseWorkDays)
            //    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

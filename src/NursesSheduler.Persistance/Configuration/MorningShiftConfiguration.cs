using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.Infrastructure.Configuration
{
    internal sealed class MorningShiftConfiguration : IEntityTypeConfiguration<MorningShift>
    {
        public void Configure(EntityTypeBuilder<MorningShift> builder)
        {
            builder.HasKey(m => m.MorningShiftId);

            //builder.HasOne(m => m.Quarter)
            //    .WithMany(q => q.MorningShifts)
            //    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using NursesScheduler.Domain.DatabaseModels.Schedules;

namespace NursesScheduler.Infrastructure.Configuration
{
    internal sealed class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            var valueComparer = new ValueComparer<ICollection<int>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList());

            builder.HasKey(s => s.ScheduleId);

            builder.Property(s => s.Holidays)
                .HasConversion(new ValueConverter<ICollection<int>, string>(
                    s => JsonConvert.SerializeObject(s),
                    s => JsonConvert.DeserializeObject<ICollection<int>>(s)));

            builder.Property(s => s.Holidays)
                .Metadata
                .SetValueComparer(valueComparer);
        }
    }
}

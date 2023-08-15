using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.Infrastructure.Configuration
{
    internal sealed class AbsenceConfiguration : IEntityTypeConfiguration<Absence>
    {
        public void Configure(EntityTypeBuilder<Absence> builder)
        {
            builder.HasKey(a => a.AbsenceId);

            //builder.HasOne(a => a.AbsencesSummary)
            //    .WithMany(s => s.Absences)
            //    .OnDelete(DeleteBehavior.NoAction);

            var valueComparer = new ValueComparer<ICollection<int>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList());

            builder.Property(a => a.Days)
                .HasConversion(new ValueConverter<ICollection<int>, string>(
                    a => JsonConvert.SerializeObject(a),
                    a => JsonConvert.DeserializeObject<ICollection<int>>(a)));

            builder.Property(s => s.Days)
                .Metadata
                .SetValueComparer(valueComparer);
        }
    }
}

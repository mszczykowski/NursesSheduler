using NursesScheduler.Domain.Abstractions;
using NursesScheduler.Domain.Enums;

namespace NursesScheduler.Domain.Entities
{
    public record MorningShift : IEntity
    {
        public MorningShiftIndex Index { get; set; }
        public TimeSpan ShiftLength { get; set; }

        public int QuarterId { get; set; }
        public virtual Quarter Quarter { get; set; }
    }
}

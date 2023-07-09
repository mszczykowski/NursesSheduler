using NursesScheduler.Domain.Enums;

namespace NursesScheduler.Domain.Entities
{
    public record MorningShift
    {
        public int MorningShiftId { get; set; }
        public int QuarterNumber { get; set; }
        public MorningShiftIndex Index { get; set; }
        public TimeSpan ShiftLength { get; set; }
    }
}

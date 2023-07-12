using NursesScheduler.Domain.Enums;

namespace NursesScheduler.Domain.Entities
{
    public record MorningShift
    {
        public int MorningShiftId { get; set; }
        public MorningShiftIndex Index { get; set; }
        public TimeSpan ShiftLength { get; set; }
        public int QuarterId { get; set; }
        public Quarter Quarter { get; set; }
    }
}

using NursesScheduler.Domain.Enums;

namespace NursesScheduler.Domain.DomainModels
{
    public sealed class MorningShift
    {
        public int MorningShiftId { get; set; }
        public MorningShiftIndex Index { get; set; }
        public TimeSpan ShiftLength { get; set; }

        public int QuarterId { get; set; }
        public Quarter Quarter { get; set; }
    }
}

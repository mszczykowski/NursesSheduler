namespace NursesScheduler.Domain.DomainModels
{
    public sealed class MorningShift
    {
        public int MorningShiftId { get; set; }
        public TimeSpan Lenght { get; set; }
    }
}

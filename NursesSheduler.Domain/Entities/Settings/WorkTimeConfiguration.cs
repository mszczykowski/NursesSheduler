namespace NursesScheduler.Domain.Entities.Settings
{
    public sealed class WorkTimeConfiguration
    {
        public TimeSpan ShiftLenght { get; } = new TimeSpan(12, 0, 0);
        public TimeSpan WorkTimePerDay { get; } = new TimeSpan(7, 35, 0);
        public TimeSpan MaximumWorkTimeInWeek { get; set; } = new TimeSpan(24, 0, 0);
        public int TargetNumberOfNursesOnShift { get; } = 4;
        public TimeSpan TargetMinimalShiftLenght { get; } = new TimeSpan(6, 0, 0);
    }
}

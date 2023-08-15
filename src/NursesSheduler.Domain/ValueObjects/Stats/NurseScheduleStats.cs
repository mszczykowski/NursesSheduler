using NursesScheduler.Domain.Enums;

namespace NursesScheduler.Domain.ValueObjects.Stats
{
    public sealed record NurseScheduleStats : NurseStats
    {
        public ShiftTypes LastState { get; set; }
        public TimeSpan HoursFromLastAssignedShift { get; set; }
        public TimeSpan HoursToFirstAssignedShift { get; set; }
    }
}

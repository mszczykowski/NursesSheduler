using NursesScheduler.Domain.Enums;

namespace NursesScheduler.Domain.ValueObjects.Stats
{
    public record NurseStats
    {
        public int NurseId { get; set; }
        public TimeSpan AssignedWorkTime { get; set; }
        public TimeSpan HolidayHoursAssigned { get; set; }
        public int NightShiftsAssigned { get; set; }
        public IDictionary<int, TimeSpan> WorkTimeInWeeks { get; set; }
        public IEnumerable<MorningShiftIndex> MorningShiftsAssigned { get; set; }
    }
}

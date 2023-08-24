namespace NursesScheduler.Domain.ValueObjects.Stats
{
    public record NurseStats
    {
        public int NurseId { get; set; }
        public TimeSpan AssignedWorkTime { get; set; }
        public TimeSpan HolidayHoursAssigned { get; set; }
        public TimeSpan TimeOffToAssign { get; set; }
        public TimeSpan TimeOffAssigned { get; set; }
        public TimeSpan NightHoursAssigned { get; set; }
        public IDictionary<int, TimeSpan> WorkTimeAssignedInWeeks { get; set; }
        public IEnumerable<int> AssignedMorningShiftsIds { get; set; }
    }
}

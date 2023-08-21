namespace NursesScheduler.Domain.ValueObjects.Stats
{
    public sealed record QuarterStats
    {
        public TimeSpan WorkTimeInQuarter { get; set; }
        public TimeSpan TimeForMorningShifts { get; set; }
        public IEnumerable<NurseStats> NurseStats { get; set; }
        public int[] ShiftsToAssignInMonths { get; set; }
    }
}

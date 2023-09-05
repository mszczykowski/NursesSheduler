namespace NursesScheduler.Domain.ValueObjects.Stats
{
    public sealed record NurseQuarterStats : NurseStats
    {
        public bool HadNumberOfShiftsReduced { get; set; }
    }
}

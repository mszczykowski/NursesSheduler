namespace NursesScheduler.Domain.DomainModels
{
    public sealed class Quarter
    {
        public int Id { get; set; }
        public int QuarterNumber { get; set; }
        public int QuarterYear { get; set; }
        public int DepartamentId { get; set; }
        public int SettingsVersion { get; set; }

        public ICollection<NurseQuarterStats> NurseQuarterStats { get; set; }
        
        public bool MorningShiftsReadOnly { get; set; }
        public ICollection<MorningShifts> MorningShifts { get; set; }
    }
}

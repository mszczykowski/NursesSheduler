namespace NursesScheduler.Domain.DomainModels
{
    public sealed class NurseQuarterStats
    {
        public TimeSpan WorkTimeInQuarterToAssign { get; set; }
        public TimeSpan WorkTimeInQuarterToAssigned { get; set; }
        public TimeSpan[] WorkTimeAssignedInWeek { get; set; }


        public TimeSpan HolidayPaidHoursAssigned { get; set; }
        public int NumberOfNightShifts { get; set; }

        public ICollection<MorningShifts> MorningShiftsAssigned { get; set; }
    }
}

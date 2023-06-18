namespace NursesScheduler.Domain.Entities
{
    public class NurseQuarterStats
    {
        public int NurseId { get; set; }
        public TimeSpan WorkTimeInQuarterToAssignLeft { get; set; }
        public ICollection<WorkTimeInWeek> WorkTimeAssignedInWeek { get; set; }
        public TimeSpan HolidayPaidHoursAssigned { get; set; }
        public int NumberOfNightShifts { get; set; }
        public ICollection<MorningShift> MorningShiftsAssigned { get; set; }

        public NurseQuarterStats(int nurseId)
        {
            NurseId = nurseId;
            WorkTimeAssignedInWeek = new HashSet<WorkTimeInWeek>();
            MorningShiftsAssigned = new HashSet<MorningShift>();
        }
    }
}

using NursesScheduler.Domain.Entities;

namespace NursesScheduler.Domain.ValueObjects
{
    public sealed record NurseQuarterStats
    {
        public int NurseId { get; set; }
        public TimeSpan WorkTimeAssignedInQuarter { get; set; }
        public ICollection<WorkTimeInWeek> WorkTimeAssignedInWeeks { get; set; }
        public TimeSpan HolidayPaidHoursAssigned { get; set; }
        public int NumberOfNightShifts { get; set; }
        public ICollection<MorningShift> MorningShiftsAssigned { get; set; }

        public NurseQuarterStats(int nurseId)
        {
            NurseId = nurseId;
            WorkTimeAssignedInWeeks = new HashSet<WorkTimeInWeek>();
            MorningShiftsAssigned = new HashSet<MorningShift>();
        }
    }
}

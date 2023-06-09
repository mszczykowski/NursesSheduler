using System.ComponentModel.DataAnnotations.Schema;

namespace NursesScheduler.Domain.Entities
{
    public class NurseQuarterStats
    {
        public int NurseQuarterStatsId { get; set; }
        public TimeSpan WorkTimeInQuarterToAssign { get; set; }

        public virtual ICollection<WorkTimeInWeek> WorkTimeAssignedInWeek { get; set; }


        public TimeSpan HolidayPaidHoursAssigned { get; set; }
        public int NumberOfNightShifts { get; set; }

        public virtual ICollection<MorningShift> MorningShiftsAssigned { get; set; }

        public int NurseId { get; set; }
        public virtual Nurse Nurse { get; set; }

        [NotMapped]
        public TimeSpan WorkTimeInQuarterToAssigned { get; set; }
    }
}

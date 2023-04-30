using NursesScheduler.Domain.Enums;

namespace NursesScheduler.Domain.DomainModels
{
    public class NurseWorkDay
    {
        public int NurseWorkDayId { get; set; }
        public int DayNumber { get; set; }

        public int ScheduleNurseId { get; set; }
        public virtual ScheduleNurse ScheduleNurse { get; set; }

        public ShiftTypes Type { get; set; }
        public TimeOnly ShiftStart { get; set; }
        public TimeOnly ShiftEnd { get; set; }

        public int? MorningShiftId { get; set; }
        public virtual MorningShift MorningShift { get; set; }

        public int? AbsenceId { get; set; }
        public virtual Absence Absence { get; set; }
    }
}

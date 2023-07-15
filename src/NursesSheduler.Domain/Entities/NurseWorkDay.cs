using NursesScheduler.Domain.Enums;

namespace NursesScheduler.Domain.Entities
{
    public record NurseWorkDay
    {
        public int NurseWorkDayId { get; set; }
        public int Day { get; set; }
        public bool IsTimeOff { get; set; }
        public ShiftTypes ShiftType { get; set; }

        public int? MorningShiftId { get; set; }
        public virtual MorningShift? MorningShift { get; set; }

        public int ScheduleNurseId { get; set; }
        public virtual ScheduleNurse ScheduleNurse { get; set; }

        
    }
}

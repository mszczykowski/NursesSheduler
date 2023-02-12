using NursesScheduler.Domain.DatabaseModels;
using NursesScheduler.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace NursesScheduler.Domain.DatabaseModels.Schedules
{
    public sealed class Shift
    {
        [Key]
        public int ShiftId { get; set; }
        public int Day { get; set; }

        public int ScheduleId { get; set; }
        public Schedule Schedule { get; set; }

        public ICollection<Nurse> AssignedNurses { get; set; }
        public ShiftTypes Type { get; set; }
        public bool IsShortShift { get; set; }
        public TimeOnly ShiftStart { get; set; }
        public TimeOnly ShiftEnd { get; set; }
    }
}

using NursesScheduler.Domain.Entities.Calendar;
using NursesScheduler.Domain.Enums;

namespace NursesScheduler.Domain.Entities.Schedules
{
    public sealed class Shift
    {
        public int Id { get; set; }
        public Day Day { get; set; }
        public Schedule Schedule { get; set; }
        public ICollection<Nurse> AssignedNurses { get; set; }
        public ShiftTypes Type { get; set; }
        public bool IsShortShift { get; set; }
        public TimeOnly ShiftStart { get; set; }
        public TimeOnly ShiftEnd { get; set; }
    }
}

using NursesScheduler.Domain.Entities.Calendar;

namespace NursesScheduler.Domain.Entities.Schedules
{
    public sealed class Schedule
    {
        public int Id { get; set; }
        public Month Month { get; set; }
        public ICollection<Shift> Shifts { get; set; }
        public ICollection<TimeOff> TimeOffs { get; set; }
    }
}

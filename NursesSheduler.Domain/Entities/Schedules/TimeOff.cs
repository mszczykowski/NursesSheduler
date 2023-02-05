using NursesScheduler.Domain.Entities.Calendar;
using NursesScheduler.Domain.Enums;

namespace NursesScheduler.Domain.Entities.Schedules
{
    public sealed class TimeOff
    {
        public int Id { get; set; }
        public Day Day { get; set; }
        public Nurse Nurse { get; set; }
        public TimeOffTypes Type { get; set; }
    }
}

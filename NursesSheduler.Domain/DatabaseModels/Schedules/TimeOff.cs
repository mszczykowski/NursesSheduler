using NursesScheduler.Domain.DatabaseModels;
using NursesScheduler.Domain.Enums;

namespace NursesScheduler.Domain.DatabaseModels.Schedules
{
    public sealed class TimeOff
    {
        public int TimeOffId { get; set; }
        public int Day { get; set; }
        public TimeOffTypes Type { get; set; }

        public int NurseId { get; set; }
        public Nurse Nurse { get; set; }
    }
}

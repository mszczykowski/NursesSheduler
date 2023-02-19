using NursesScheduler.Domain.Enums;

namespace NursesScheduler.Domain.DatabaseModels
{
    public sealed class TimeOff
    {
        public int TimeOffId { get; set; }
        public DateOnly From { get; set; }
        public DateOnly To { get; set; }
        public TimeOffTypes Type { get; set; }

        public int NurseId { get; set; }
        public Nurse Nurse { get; set; }
    }
}

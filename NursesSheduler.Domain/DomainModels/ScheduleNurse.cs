namespace NursesScheduler.Domain.DomainModels
{
    public sealed class ScheduleNurse
    {
        public int ScheduleNurseId { get; set; }

        public ICollection<Shift> Shifts { get; set; }

        public int NurseId { get; set; }
        public Nurse Nurse { get; set; }

        public int ScheduleId { get; set; }
        public Schedule Schedule { get; set; }

    }
}

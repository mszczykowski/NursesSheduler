namespace NursesScheduler.BlazorShared.Models.ViewModels.Entities
{
    public sealed class ScheduleNurseViewModel
    {
        public int ScheduleNurseId { get; set; }
        public int NurseId { get; set; }

        public IEnumerable<NurseWorkDayViewModel> NurseWorkDays { get; set; }
    }
}

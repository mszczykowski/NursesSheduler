namespace NursesScheduler.BlazorShared.Models.ViewModels
{
    public sealed class ScheduleNurseViewModel
    {
        public int ScheduleNurseId { get; set; }
        public int NurseId { get; set; }

        public IEnumerable<NurseWorkDayViewModel> NurseWorkDays { get; set; }
    }
}

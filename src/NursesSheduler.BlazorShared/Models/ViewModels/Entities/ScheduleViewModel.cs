namespace NursesScheduler.BlazorShared.Models.ViewModels.Entities
{
    public sealed class ScheduleViewModel
    {
        public int ScheduleId { get; set; }
        public int Month { get; set; }
        public int QuarterId { get; set; }
        public bool IsClosed { get; set; }
        public ICollection<ScheduleNurseViewModel> ScheduleNurses { get; set; }
    }
}

using NursesScheduler.BlazorShared.ViewModels.Enums;

namespace NursesScheduler.BlazorShared.ViewModels
{
    public sealed class ScheduleNurseViewModel
    {
        public int ScheduleNurseId { get; set; }
        public int NurseId { get; set; }

        public IEnumerable<NurseWorkDayViewModel> NurseWorkDays { get; set; }
    }
}

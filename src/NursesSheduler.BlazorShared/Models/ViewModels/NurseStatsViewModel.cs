namespace NursesScheduler.BlazorShared.Models.ViewModels
{
    public sealed class NurseStatsViewModel
    {
        public int NurseId { get; set; }
        public TimeSpan AssignedWorkTime { get; set; }
        public TimeSpan HolidayHoursAssigned { get; set; }
        public TimeSpan TimeOffToAssign { get; set; }
        public TimeSpan TimeOffAssigned { get; set; }
        public int NightShiftsAssigned { get; set; }
    }
}

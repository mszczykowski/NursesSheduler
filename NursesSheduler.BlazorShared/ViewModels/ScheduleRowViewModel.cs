using NursesScheduler.BlazorShared.ViewModels.Enums;

namespace NursesScheduler.BlazorShared.ViewModels
{
    public sealed class ScheduleRowViewModel
    {
        public NurseViewModel Nurse { get; init; }
        public PreviousStatates PreviousState { get; set; }
        public ShiftViewModel[] Shifts { get; }
        public TimeSpan PreviousMonthTime { get; init; }
        public TimeSpan AssignedTime { get; set; }
        public TimeSpan AssignedTimeInQuarter { get; set; }
        public TimeSpan AssignedPTO { get; set; }
    }
}

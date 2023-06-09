using NursesScheduler.BlazorShared.ViewModels.Enums;

namespace NursesScheduler.BlazorShared.ViewModels
{
    public sealed class ScheduleNurseViewModel
    {
        public int ScheduleNurseId { get; set; }
        public int NurseId { get; set; }
        public NurseViewModel Nurse { get; set; }
        public PreviousStatates PreviousState { get; set; }
        public int DaysFromLastShift { get; set; }

        public NurseWorkDayViewModel[] NurseWorkDays { get; set; }
        public TimeSpan PreviousMonthTime { get; set; }
        public TimeSpan TimeToAssingInMonth { get; set; }
        public TimeSpan TimeToAssingInQuarterLeft { get; set; }
        public TimeSpan TimeOffToAssign { get; set; }
    }
}

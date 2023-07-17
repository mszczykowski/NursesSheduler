using NursesScheduler.BlazorShared.ViewModels.Enums;

namespace NursesScheduler.BlazorShared.ViewModels.Common
{
    public sealed class ScheduleRowCommonDataViewModel
    {
        public bool ReadOnly { get; set; }
        public StatsDisplayed StatsDipslayed { get; set; }
        public int DepartamentId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public TimeSpan QuarterWorkTime { get; set; }
        public TimeSpan MonthWorkTime { get; set; }
        public IEnumerable<DayViewModel> MonthDays { get; set; }
        public IEnumerable<MorningShiftViewModel> MorningShifts { get; set; }
    }
}

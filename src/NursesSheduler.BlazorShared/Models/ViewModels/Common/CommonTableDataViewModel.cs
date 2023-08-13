using NursesScheduler.BlazorShared.Models.Enums;
using NursesScheduler.BlazorShared.Models.ViewModels.Entities;
using NursesScheduler.BlazorShared.Models.ViewModels.ValueObjects;

namespace NursesScheduler.BlazorShared.Models.ViewModels.Common
{
    public sealed class CommonTableDataViewModel
    {
        public bool ReadOnly { get; set; }
        public StatsDisplayed CurrentStatsDipslayed { get; set; }
        public int DepartamentId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public TimeSpan QuarterWorkTime { get; set; }
        public TimeSpan MonthWorkTime { get; set; }
        public IEnumerable<DayViewModel> MonthDays { get; set; }
        public IEnumerable<MorningShiftViewModel> MorningShifts { get; set; }

        public event Action TableNeedsRefresing;

        public void RequestTableRefresh()
        {
            TableNeedsRefresing.Invoke();
        }
    }
}

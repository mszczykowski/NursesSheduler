using NursesScheduler.BlazorShared.Models.Enums;
using NursesScheduler.BlazorShared.Models.ViewModels;
using NursesScheduler.BlazorShared.Models.ViewModels.ValueObjects;

namespace NursesScheduler.BlazorShared.Models.Wrappers
{
    public sealed class ScheduleDataWrapper
    {
        public ScheduleStatsViewModel ScheduleStats { get; set; }
        public IEnumerable<DayViewModel> Days { get; set; }
        public ScheduleViewModel Schedule { get; set; }
        public bool ReadOnly { get; set; }
        public StatsDisplayed CurrentStatsDipslayed { get; set; }


        public event Action RefreshScheduleView;
        public event Action RecalculateScheduleStats;

        public void RequestScheduleViewRefresh()
        {
            RefreshScheduleView.Invoke();
        }

        public void RequestScheduleRecalculation()
        {
            RecalculateScheduleStats.Invoke();
        }
    }
}

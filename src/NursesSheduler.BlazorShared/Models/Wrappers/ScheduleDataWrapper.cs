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


        public event Action ScheduleNeedsRefreshing;
        public event Action ScheduleNeedsRecalculation;

        public void RequestScheduleViewRefresh()
        {
            ScheduleNeedsRefreshing.Invoke();
        }

        public void RequestScheduleRecalculation()
        {
            ScheduleNeedsRecalculation.Invoke();
        }
    }
}

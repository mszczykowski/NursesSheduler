using NursesScheduler.BlazorShared.Models.Enums;
using NursesScheduler.BlazorShared.Models.ViewModels.Entities;
using NursesScheduler.BlazorShared.Models.ViewModels.ValueObjects;

namespace NursesScheduler.BlazorShared.Models.Wrappers
{
    public sealed class ScheduleDataWrapper
    {
        public ScheduleStatsViewModel ScheduleStats { get; set; }
        public IEnumerable<DayViewModel> Days { get; set; }
        public ScheduleViewModel Schedule { get; set; }
        public IDictionary<int, IEnumerable<ScheduleValidationErrorViewModel>> ValidationErrors { get; set; }
        public bool ReadOnly { get; set; }
        public StatsDisplayed CurrentStatsDipslayed { get; set; }


        public event Action RefreshScheduleView;
        public event Action RecalculateScheduleStats;
        public event Action<int> RecalculateRowStats;

        public void RequestScheduleViewRefresh()
        {
            RefreshScheduleView.Invoke();
        }

        public void RequestScheduleRecalculation()
        {
            RecalculateScheduleStats.Invoke();
        }

        public void RequestRowRecalculation(int nurseId)
        {
            RecalculateRowStats.Invoke(nurseId);
        }
    }
}

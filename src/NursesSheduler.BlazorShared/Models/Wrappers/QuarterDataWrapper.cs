using NursesScheduler.BlazorShared.Models.ViewModels;
using NursesScheduler.BlazorShared.Models.ViewModels.ValueObjects;

namespace NursesScheduler.BlazorShared.Models.Wrappers
{
    public sealed class QuarterDataWrapper
    {
        public QuarterViewModel Quarter { get; set; }
        public QuarterStatsViewModel QuarterStats { get; set; }
        public IEnumerable<MorningShiftViewModel> MorningShifts { get; set; }
    }
}

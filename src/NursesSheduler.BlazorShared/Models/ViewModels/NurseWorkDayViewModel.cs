using NursesScheduler.BlazorShared.Models.Enums;

namespace NursesScheduler.BlazorShared.Models.ViewModels
{
    public sealed class NurseWorkDayViewModel
    {
        public int NurseWorkDayId { get; set; }
        public int Day { get; set; }
        public bool IsTimeOff { get; set; }
        public ShiftTypes ShiftType { get; set; }
        public MorningShiftIndexes MorningShiftIndex { get; set; }
    }
}

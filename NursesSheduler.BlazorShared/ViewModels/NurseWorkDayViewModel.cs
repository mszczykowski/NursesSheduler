using NursesScheduler.BlazorShared.ViewModels.Enums;

namespace NursesScheduler.BlazorShared.ViewModels
{
    public sealed class NurseWorkDayViewModel
    {
        public int NurseWorkDayId { get; set; }
        public ShiftTypes ShiftType { get; set; }
        public int DayNumber { get; set; }
        public TimeOnly ShiftStart { get; set; }
        public TimeOnly ShiftEnd { get; set; }
        public bool IsTimeOff { get; set; }
        public int MorningShiftId { get; set; }
    }
}

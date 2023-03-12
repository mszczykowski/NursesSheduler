using NursesScheduler.BlazorShared.ViewModels.Enums;

namespace NursesScheduler.BlazorShared.ViewModels
{
    public sealed class ShiftViewModel
    {
        public ShiftTypes ShiftType { get; set; }
        public TimeOnly ShiftStart { get; set; }
        public TimeSpan ShiftEnd { get; set; }
        public override string ToString()
        {
            return ShiftStart + " : " + ShiftEnd;
        }

        public bool IsTimeOff { get; set; }
        public AbsenceTypes TimeOffType { get; set; }
    }
}

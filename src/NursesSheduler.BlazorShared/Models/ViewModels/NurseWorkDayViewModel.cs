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
        public MorningShiftViewModel? MorningShift { get; set; }

        public void SetShift(ShiftTypes type)
        {
            ShiftType = type;
            MorningShift = null;
        }

        public void RefreshMorningShift(IEnumerable<MorningShiftViewModel> morningShifts)
        {
            if(ShiftType == ShiftTypes.Morning)
            {
                MorningShift = morningShifts.First(m => m.Index == MorningShiftIndex);
            }
        }

        public void SetMorningShift(MorningShiftIndexes index, IEnumerable<MorningShiftViewModel> morningShifts)
        {
            ShiftType = ShiftTypes.Morning;
            MorningShiftIndex = index;
            MorningShift = morningShifts.First(m => m.Index == index);
        }
    }
}

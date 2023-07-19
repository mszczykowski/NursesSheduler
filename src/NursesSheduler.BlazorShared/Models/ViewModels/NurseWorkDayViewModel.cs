using NursesScheduler.BlazorShared.Models.Enums;

namespace NursesScheduler.BlazorShared.Models.ViewModels
{
    public sealed class NurseWorkDayViewModel
    {
        public int NurseWorkDayId { get; set; }
        public int Day { get; set; }
        public bool IsTimeOff { get; set; }
        public ShiftTypes ShiftType { get; set; }
        public int MorningShiftId { get; set; }
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
                MorningShift = morningShifts.First(m => m.MorningShiftId == MorningShiftId);
            }
        }

        public void SetMorningShift(MorningShiftIndexes index, IEnumerable<MorningShiftViewModel> morningShifts)
        {
            ShiftType = ShiftTypes.Morning;
            MorningShift = morningShifts.First(m => m.Index == index);
            MorningShiftId = MorningShiftId;
        }
    }
}

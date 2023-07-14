using NursesScheduler.BlazorShared.ViewModels.Enums;
using System.ComponentModel.DataAnnotations;

namespace NursesScheduler.BlazorShared.ViewModels.Forms
{
    public sealed class MorningShiftsFormViewModel
    {
        [ValidateComplexType]
        public MorningShiftViewModel[] MorningShifts { get; set; }

        public MorningShiftsFormViewModel(IEnumerable<MorningShiftViewModel> morningShifts)
        {
            MorningShifts = new MorningShiftViewModel[3];

            for (int i = 0; i < MorningShifts.Length; i++)
            {
                MorningShifts[i] = new MorningShiftViewModel
                {
                    ShiftLength = TimeSpan.Zero,
                    Index = (MorningShiftIndexes)i,
                };
            }

            if (morningShifts is not null && morningShifts.Any())
            {
                foreach (var morningShift in morningShifts)
                {
                    MorningShifts.First(m => m.Index == morningShift.Index).ShiftLength = morningShift.ShiftLength;
                }
            }
        }
    }
}

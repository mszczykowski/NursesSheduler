using NursesScheduler.BlazorShared.ViewModels.Enums;
using System.ComponentModel.DataAnnotations;

namespace NursesScheduler.BlazorShared.ViewModels
{
    public sealed class MorningShiftsEditViewModel : IValidatableObject
    {
        public TimeSpan TimeToDivide { get; set; }
        [ValidateComplexType]
        public MorningShiftViewModel[] MorningShifts { get; set; }

        public MorningShiftsEditViewModel(MorningShiftViewModel[] morningShifts, TimeSpan timeToDivide)
        {
            TimeToDivide = timeToDivide;

            MorningShifts = new MorningShiftViewModel[3];

            for (int i = 0; i < MorningShifts.Length; i++)
            {
                MorningShifts[i] = new MorningShiftViewModel
                {
                    ShiftLength = TimeSpan.Zero,
                    Index = (MorningShiftIndexes)i,
                };
            }

            if (morningShifts != null && morningShifts.Any())
            {
                foreach (var morningShift in morningShifts)
                {
                    MorningShifts.First(m => m.Index == morningShift.Index).ShiftLength = morningShift.ShiftLength;
                }
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var sum = TimeSpan.Zero;
            foreach (var shift in MorningShifts)
            {
                sum += shift.ShiftLength;
            }
            if (sum < TimeToDivide)
                yield return new ValidationResult("Należy wykorzystać cały czas do podziału");

            /*for (int i = 0; i < MorningShifts.Length; i++)
            {
                if(MorningShifts[i].ShiftLength == TimeSpan.Zero)
                {
                    for(int j = i + 1; j < MorningShifts.Length; j++)
                    {
                        if(MorningShifts[j].ShiftLength != TimeSpan.Zero)
                            yield return new ValidationResult("Długości należy uzupełniać w kolejności od 1 do 3");
                    }
                }
            }*/
        }
    }
}

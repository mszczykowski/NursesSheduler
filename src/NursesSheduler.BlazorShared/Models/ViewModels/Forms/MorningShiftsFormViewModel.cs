﻿using NursesScheduler.BlazorShared.Models.Enums;
using NursesScheduler.BlazorShared.Models.ViewModels.Entities;
using System.ComponentModel.DataAnnotations;

namespace NursesScheduler.BlazorShared.Models.ViewModels.Forms
{
    public sealed class MorningShiftsFormViewModel
    {
        [ValidateComplexType]
        public MorningShiftViewModel[] MorningShifts { get; set; }

        public MorningShiftsFormViewModel(IEnumerable<MorningShiftViewModel> morningShifts)
        {
            var numberOfMorningShifts = Enum.GetValues<MorningShiftIndexes>().Length;

            MorningShifts = new MorningShiftViewModel[numberOfMorningShifts];

            for (int i = 0; i < numberOfMorningShifts; i++)
            {
                MorningShifts[i] = new MorningShiftViewModel
                {
                    ShiftLength = TimeSpan.Zero,
                    Index = (MorningShiftIndexes)i,
                };
            }

            SetMorningShifts(morningShifts);
        }

        private void SetMorningShifts(IEnumerable<MorningShiftViewModel> morningShifts)
        {
            if (morningShifts is not null && morningShifts.Any())
            {
                foreach (var morningShift in morningShifts)
                {
                    var matchingMorningShift = MorningShifts.First(m => m.Index == morningShift.Index);
                    matchingMorningShift.ShiftLength = morningShift.ShiftLength;
                    matchingMorningShift.ReadOnly = morningShift.ReadOnly;
                }
            }
        }

        public bool IsDirty(IEnumerable<MorningShiftViewModel> unmodifiedShifts)
        {
            return MorningShifts.Any(m => unmodifiedShifts.Any(u => u.Index == m.Index && u.ShiftLength != m.ShiftLength))
                || (!unmodifiedShifts.Any() && MorningShifts.Any(m => m.ShiftLength != TimeSpan.Zero));
        }
    }
}

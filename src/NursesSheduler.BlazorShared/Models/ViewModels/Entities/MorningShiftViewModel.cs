using NursesScheduler.BlazorShared.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace NursesScheduler.BlazorShared.Models.ViewModels.Entities
{
    public sealed class MorningShiftViewModel
    {
        public int MorningShiftId { get; set; }
        public MorningShiftIndexes Index { get; set; }
        [Range(typeof(TimeSpan), "00:00:00", "11:59:00", ErrorMessage = "Długość musi być mniejsza niż 12h")]
        public TimeSpan ShiftLength { get; set; }
        public bool ReadOnly { get; set; }
    }
}
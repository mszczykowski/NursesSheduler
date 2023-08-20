using NursesScheduler.BlazorShared.Models.Enums;

namespace NursesScheduler.BlazorShared.Models.ViewModels.ValueObjects
{
    public sealed class ScheduleValidationErrorViewModel
    {
        public int NurseId { get; set; }
        public ScheduleInvalidReasons Reason { get; set; }
        public string? AdditionalInfo { get; set; }
    }
}

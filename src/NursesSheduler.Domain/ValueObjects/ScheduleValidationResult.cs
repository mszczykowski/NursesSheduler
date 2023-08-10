using NursesScheduler.Domain.Enums;

namespace NursesScheduler.Domain.ValueObjects
{
    public sealed class ScheduleValidationResult
    {
        public int NurseId { get; set; }
        public ScheduleInvalidReasons Reason { get; set; }
        public string? AdditionalInfo { get; set; }
    }
}

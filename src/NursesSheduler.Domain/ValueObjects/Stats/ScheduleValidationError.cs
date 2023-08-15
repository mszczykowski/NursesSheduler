using NursesScheduler.Domain.Enums;

namespace NursesScheduler.Domain.ValueObjects
{
    public sealed class ScheduleValidationError
    {
        public ScheduleInvalidReasons Reason { get; set; }
        public string? AdditionalInfo { get; set; }
    }
}

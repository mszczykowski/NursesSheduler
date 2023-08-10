using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.ScheduleValidations.Queries.ValidateScheduleNurse
{
    public sealed class ValidateScheduleNurseResponse
    {
        public int NurseId { get; set; }
        public ScheduleInvalidReasons Reason { get; set; }
        public string? AdditionalInfo { get; set; }
    }
}

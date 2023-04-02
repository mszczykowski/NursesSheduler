using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Commands.AddAbsence
{
    public sealed class AddAbsenceResponse
    {
        public int AbsenceId { get; set; }
        public DateOnly From { get; set; }
        public DateOnly To { get; set; }
        public TimeSpan WorkingHoursToAssign { get; set; }
        public AbsenceTypes Type { get; set; }
        public AbsenceVeryficationResult VeryficationResult { get; set; }
    }
}

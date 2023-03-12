using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Commands.EditAbsence
{
    internal class EditAbsenceResponse
    {
        public int AbsenceId { get; set; }
        public DateOnly From { get; set; }
        public DateOnly To { get; set; }
        public TimeSpan AssignedWorkingHours { get; set; }
        public AbsenceTypes Type { get; set; }
        public AbsenceVeryficationResult VeryficationResult { get; set; }
    }
}

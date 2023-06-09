using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Commands.AddAbsence
{
    public sealed class AddAbsenceResponse
    {
        public AbsenceVeryficationResult VeryficationResult { get; }
        public AddAbsenceResponse(AbsenceVeryficationResult veryficationResult)
        {
            VeryficationResult = veryficationResult;
        }
    }
}

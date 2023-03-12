using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Commands.DeleteAbsence
{
    public sealed class DeleteAbsenceRequest : IRequest<DeleteAbsenceResponse>
    {
        public int AbsenceId { get; set; }
    }
}

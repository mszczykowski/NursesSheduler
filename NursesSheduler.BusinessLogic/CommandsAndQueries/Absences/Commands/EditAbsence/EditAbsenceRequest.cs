using MediatR;
using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Commands.EditAbsence
{
    public sealed class EditAbsenceRequest : IRequest<EditAbsenceResponse>
    {
        public int AbsencesSummaryId { get; set; }
        public int AbsenceId { get; set; }
        public DateOnly From { get; set; }
        public DateOnly To { get; set; }
        public AbsenceTypes Type { get; set; }
    }
}

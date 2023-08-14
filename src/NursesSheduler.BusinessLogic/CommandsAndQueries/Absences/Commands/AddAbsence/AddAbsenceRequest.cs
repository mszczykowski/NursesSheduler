using MediatR;
using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Commands.AddAbsence
{
    public sealed class AddAbsenceRequest : IRequest<AddAbsenceResponse>
    {
        public int NurseId { get; set; }
        public DateOnly From { get; set; }
        public DateOnly To { get; set; }
        public AbsenceTypes Type { get; set; }
    }
}

using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Queries.GetAbsences
{
    public sealed class GetAbsencesRequest : IRequest<ICollection<GetAbsencesResponse>>
    {
        public int AbsencesSummaryId { get; set; }
    }
}

using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Queries
{
    public sealed class GetAbsencesRequest : IRequest<ICollection<GetAbsencesResponse>>
    {
        public int AbsencesSummaryId { get; set; }
    }
}

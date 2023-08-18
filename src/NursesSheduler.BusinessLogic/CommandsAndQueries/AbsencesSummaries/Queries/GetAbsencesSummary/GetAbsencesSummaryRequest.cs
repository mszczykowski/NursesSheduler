using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Queries.GetAbsencesSummary
{
    public sealed class GetAbsencesSummaryRequest : IRequest<IEnumerable<GetAbsencesSummaryResponse>>
    {
        public int NurseId { get; set; }
    }
}

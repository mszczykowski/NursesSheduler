using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Queries.GetYearlyAbsencesSummary
{
    public sealed class GetAbsencesSummaryRequest : IRequest<ICollection<GetAbsencesSummaryResponse>>
    {
        public int NurseId { get; set; }
    }
}

using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.YearlyAbsencesSummaries.Queries.GetYearlyAbsencesSummary
{
    public sealed class GetYearlyAbsencesSummaryRequest : IRequest<ICollection<GetYearlyAbsencesSummaryResponse>>
    {
        public int NurseId { get; set; }
    }
}

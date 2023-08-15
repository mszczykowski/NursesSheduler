using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Queries.GetAbsencesSummaryByDepartament
{
    public sealed class GetAbsencesSummaryByDepartamentRequest : IRequest<IEnumerable<GetAbsencesSummaryByDepartamentResponse>>
    {
        public int DepartamentId { get; set; }
    }
}

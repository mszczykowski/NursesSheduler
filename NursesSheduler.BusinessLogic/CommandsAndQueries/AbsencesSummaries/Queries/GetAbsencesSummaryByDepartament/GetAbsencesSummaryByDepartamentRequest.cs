using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Queries.GetAbsencesSummaryByDepartament
{
    public sealed class GetAbsencesSummaryByDepartamentRequest 
                                                        : IRequest<ICollection<GetAbsencesSummaryByDepartamentResponse>>
    {
        public int DepartamentId { get; set; }
    }
}

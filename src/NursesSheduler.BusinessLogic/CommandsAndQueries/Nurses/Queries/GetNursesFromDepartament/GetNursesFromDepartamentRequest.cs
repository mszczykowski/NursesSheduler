using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Nurses.Queries.GetNursesFromDepartament
{
    public sealed class GetNursesFromDepartamentRequest : IRequest<List<GetNursesFromDepartamentResponse>>
    {
        public int DepartamentId { get; set; }
    }
}

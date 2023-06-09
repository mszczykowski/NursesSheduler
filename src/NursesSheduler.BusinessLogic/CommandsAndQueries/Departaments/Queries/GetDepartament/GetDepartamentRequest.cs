
using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Queries.GetDepartament
{
    public sealed class GetDepartamentRequest : IRequest<GetDepartamentResponse>
    {
        public int DepartamentId { get; set; }
    }
}

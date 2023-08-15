using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Queries.PickDepartament
{
    public sealed class PickDepartamentRequest : IRequest<PickDepartamentResponse>
    {
        public int DepartamentId { get; set; }
    }
}

using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Commands.PickDepartament
{
    public sealed class PickDepartamentRequest : IRequest<PickDepartamentResponse>
    {
        public int DepartamentId { get; set; }
    }
}

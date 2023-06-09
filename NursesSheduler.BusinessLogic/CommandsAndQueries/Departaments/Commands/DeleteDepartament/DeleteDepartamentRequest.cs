using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Commands.DeleteDepartament
{
    public sealed class DeleteDepartamentRequest : IRequest<DeleteDepartamentResponse>
    {
        public int DepartamentId { get; set; }
    }
}

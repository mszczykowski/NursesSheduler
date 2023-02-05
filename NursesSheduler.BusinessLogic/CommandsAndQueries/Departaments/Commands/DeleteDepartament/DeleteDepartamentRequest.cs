using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Commands.DeleteDepartament
{
    public class DeleteDepartamentRequest : IRequest<DeleteDepartamentResponse>
    {
        public int Id { get; set; }
    }
}

using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Commands.EditDepartament
{
    public class EditDepartamentRequest : IRequest<EditDepartamentResponse>
    {
        public int DepartamentId { get; set; }
        public string Name { get; set; }
        public int CreationYear { get; set; }
    }
}

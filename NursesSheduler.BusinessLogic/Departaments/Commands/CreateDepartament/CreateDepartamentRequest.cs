using MediatR;

namespace NursesScheduler.BusinessLogic.Departaments.Commands.CreateDepartament
{
    public class CreateDepartamentRequest : IRequest<CreateDepartamentResponse>
    {
        public string Name { get; set; }
    }
}

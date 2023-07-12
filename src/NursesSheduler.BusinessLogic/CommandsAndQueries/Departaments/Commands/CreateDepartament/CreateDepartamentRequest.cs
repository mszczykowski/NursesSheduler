using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Commands.CreateDepartament
{
    public class CreateDepartamentRequest : IRequest<CreateDepartamentResponse>
    {
        public string Name { get; set; }
        public int FirstQuarterStart { get; set; }
    }
}

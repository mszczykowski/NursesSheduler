using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Nurses.Commands.AddNurse
{
    public sealed class AddNurseRequest : IRequest<AddNurseResponse>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int PTOentitlement { get; set; }
        public int DepartamentId { get; set; }

    }
}

using MediatR;
using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Nurses.Commands.AddNurse
{
    public sealed class AddNurseRequest : IRequest<AddNurseResponse>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int PTOentitlement { get; set; }
        public NurseTeams Team { get; set; }
        public int DepartamentId { get; set; }

    }
}

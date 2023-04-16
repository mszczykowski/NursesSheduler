using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Nurses.Commands.EditNurse
{
    public sealed class EditNurseRequest : IRequest<EditNurseResponse>
    {
        public int NurseId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int PTOentitlement { get; set; }
    }
}

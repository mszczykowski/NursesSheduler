using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Nurses.Commands.DeleteNurse
{
    public sealed class DeleteNurseRequest : IRequest<DeleteNurseResponse>
    {
        public int NurseId { get; set; }
    }
}

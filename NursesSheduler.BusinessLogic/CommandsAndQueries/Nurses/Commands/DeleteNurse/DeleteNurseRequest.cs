using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Nurses.Commands.DeleteNurse
{
    public class DeleteNurseRequest : IRequest<DeleteNurseResponse>
    {
        public int NurseId { get; set; }
    }
}

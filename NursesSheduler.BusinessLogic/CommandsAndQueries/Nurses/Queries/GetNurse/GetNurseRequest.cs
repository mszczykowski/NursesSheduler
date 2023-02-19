using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Nurses.Queries.GetNurse
{
    public sealed class GetNurseRequest : IRequest<GetNurseResponse>
    {
        public int NurseId { get; set; }
    }
}

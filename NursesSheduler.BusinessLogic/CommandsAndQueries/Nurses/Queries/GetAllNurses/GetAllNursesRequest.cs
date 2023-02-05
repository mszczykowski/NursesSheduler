using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Nurses.Queries.GetAllNurses
{
    public class GetAllNursesRequest : IRequest<List<GetAllNursesResponse>>
    {

    }
}

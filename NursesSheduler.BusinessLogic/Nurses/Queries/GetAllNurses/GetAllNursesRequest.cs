using MediatR;

namespace NursesScheduler.BusinessLogic.Nurses.Queries.GetAllNurses
{
    public class GetAllNursesRequest : IRequest<List<GetAllNursesResponse>>
    {

    }
}

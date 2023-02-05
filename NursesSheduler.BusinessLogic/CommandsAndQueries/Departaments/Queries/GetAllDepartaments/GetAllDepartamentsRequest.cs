using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Queries.GetAllDepartaments
{
    public class GetAllDepartamentsRequest : IRequest<List<GetAllDepartamentsResponse>>
    {

    }
}

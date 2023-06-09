using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Queries.GetAllDepartaments
{
    public sealed class GetAllDepartamentsRequest : IRequest<List<GetAllDepartamentsResponse>>
    {

    }
}

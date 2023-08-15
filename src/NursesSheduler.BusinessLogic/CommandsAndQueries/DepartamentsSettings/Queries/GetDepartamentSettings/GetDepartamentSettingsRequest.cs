using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.DepartamentsSettings.Queries.GetDepartamentSettings
{
    public sealed class GetDepartamentSettingsRequest : IRequest<GetDepartamentSettingsResponse>
    {
        public int DepartamentId { get; set; }
    }
}

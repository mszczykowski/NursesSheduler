using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.ScheduleStats.Queries.GetScheduleStatsQuery
{
    public sealed class GetScheduleStatsRequest : IRequest<GetScheduleStatsResponse>
    {
        public int Month { get; set; }
        public int QuarterId { get; set; }
    }
}

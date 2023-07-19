using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStats
{
    public sealed class GetScheduleStatsRequest : IRequest<GetScheduleStatsResponse>
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int DepartamentId { get; set; }
    }
}

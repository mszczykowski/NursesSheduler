using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule
{
    public sealed class GetScheduleRequest : IRequest<GetScheduleResponse?>
    {
        public int Month { get; set; }
        public int QuarterId { get; set; }
    }
}

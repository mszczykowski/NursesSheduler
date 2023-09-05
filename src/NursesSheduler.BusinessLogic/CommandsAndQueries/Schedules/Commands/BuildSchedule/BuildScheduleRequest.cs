using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Commands.BuildSchedule
{
    public class BuildScheduleRequest : IRequest<BuildScheduleResponse>
    {
        public int Month { get; set; }
        public int QuarterId { get; set; }
    }
}

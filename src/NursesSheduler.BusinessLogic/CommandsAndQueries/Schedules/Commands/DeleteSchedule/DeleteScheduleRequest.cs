using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Commands.DeleteSchedule
{
    public sealed class DeleteScheduleRequest : IRequest<DeleteScheduleResponse>
    {
        public int ScheduleId { get; set; }
    }
}

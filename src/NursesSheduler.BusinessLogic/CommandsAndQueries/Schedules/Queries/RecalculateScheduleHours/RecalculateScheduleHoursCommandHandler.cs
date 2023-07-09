using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Commands.RecalculateScheduleHours
{
    internal sealed class RecalculateScheduleHoursCommandHandler : IRequestHandler<RecalculateScheduleHoursRequest,
        RecalculateScheduleHoursResponse>
    {
        public Task<RecalculateScheduleHoursResponse> Handle(RecalculateScheduleHoursRequest request, 
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

using MediatR;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Exceptions;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Commands.DeleteSchedule
{
    internal sealed class DeleteScheduleCommandHandler : IRequestHandler<DeleteScheduleRequest, DeleteScheduleResponse>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public DeleteScheduleCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<DeleteScheduleResponse> Handle(DeleteScheduleRequest request, CancellationToken cancellationToken)
        {
            var schedule = await _applicationDbContext.Schedules.FindAsync(request.ScheduleId)
                ?? throw new EntityNotFoundException(request.ScheduleId, nameof(Schedule));

            _applicationDbContext.Schedules.Remove(schedule);

            var result = await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return result > 0 ? new DeleteScheduleResponse(true) : new DeleteScheduleResponse(false);
        }
    }
}

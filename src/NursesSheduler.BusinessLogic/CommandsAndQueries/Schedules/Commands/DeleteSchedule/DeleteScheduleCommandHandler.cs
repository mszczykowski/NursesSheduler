using MediatR;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Services;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Commands.DeleteSchedule
{
    internal sealed class DeleteScheduleCommandHandler : IRequestHandler<DeleteScheduleRequest, DeleteScheduleResponse>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ISchedulesService _schedulesService;

        public DeleteScheduleCommandHandler(IApplicationDbContext applicationDbContext, ISchedulesService schedulesService)
        {
            _applicationDbContext = applicationDbContext;
            _schedulesService = schedulesService;
        }

        public async Task<DeleteScheduleResponse> Handle(DeleteScheduleRequest request, CancellationToken cancellationToken)
        {
            _schedulesService.DeleteSchedule(request.ScheduleId);

            var result = await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return result > 0 ? new DeleteScheduleResponse(true) : new DeleteScheduleResponse(false);
        }
    }
}

using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule
{
    internal sealed class GetScheduleQueryHandler : IRequestHandler<GetScheduleRequest, GetScheduleResponse?>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public Task<GetScheduleResponse?> Handle(GetScheduleRequest request, CancellationToken cancellationToken)
        {
            var schedule = await _context.Schedules
                .Include(s => s.ScheduleNurses)
                .ThenInclude(n => n.NurseWorkDays)
                .FirstOrDefaultAsync(s => s.QuarterId == request.QuarterId && s.Month == request.Month);

            if (schedule is not null)
            {
                return _mapper.Map<BuildScheduleResponse>(schedule);
            }
        }
    }
}

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStatsQuery
{
    internal sealed class GetScheduleStatsFromScheduleQueryHandler : IRequestHandler<GetScheduleStatsFromScheduleRequest,
        GetScheduleStatsFromScheduleResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IScheduleStatsService _scheduleStatsService;

        public GetScheduleStatsFromScheduleQueryHandler(IApplicationDbContext context, IMapper mapper, IScheduleStatsService statsService)
        {
            _context = context;
            _mapper = mapper;
            _scheduleStatsService = statsService;
        }

        public async Task<GetScheduleStatsFromScheduleResponse> Handle(GetScheduleStatsFromScheduleRequest request,
            CancellationToken cancellationToken)
        {
            var schedule = _mapper.Map<Schedule>(request.Schedule);

            if (schedule.IsClosed)
            {
                schedule = await _context.Schedules
                    .Include(s => s.ScheduleNurses)
                    .ThenInclude(n => n.NurseWorkDays)
                    .FirstAsync(s => s.ScheduleId == schedule.ScheduleId);
            }

            return _mapper.Map<GetScheduleStatsFromScheduleResponse>(await _scheduleStatsService.GetScheduleStatsAsync(schedule,
                request.DepartamentId, request.Year));
        }
    }
}

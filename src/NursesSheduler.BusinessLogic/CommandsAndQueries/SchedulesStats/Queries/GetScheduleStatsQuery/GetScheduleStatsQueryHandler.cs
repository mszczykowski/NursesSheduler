using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStatsQuery
{
    internal sealed class GetScheduleStatsQueryHandler : IRequestHandler<GetScheduleStatsRequest,
        GetScheduleStatsResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStatsService _statsService;

        public GetScheduleStatsQueryHandler(IApplicationDbContext context, IMapper mapper, IStatsService statsService)
        {
            _context = context;
            _mapper = mapper;
            _statsService = statsService;
        }

        public async Task<GetScheduleStatsResponse> Handle(GetScheduleStatsRequest request,
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

            return _mapper.Map<GetScheduleStatsResponse>(await _statsService.GetScheduleStatsAsync(schedule,
                request.DepartamentId, request.Year));
        }
    }
}

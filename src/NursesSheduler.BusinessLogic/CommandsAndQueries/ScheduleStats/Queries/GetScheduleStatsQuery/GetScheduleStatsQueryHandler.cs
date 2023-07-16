using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.BusinessLogic.Exceptions;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.ScheduleStats.Queries.GetScheduleStatsQuery
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
            var schedule = await _context.Schedules
                .Include(s => s.ScheduleNurses)
                .ThenInclude(n => n.NurseWorkDays)
                .FirstOrDefaultAsync(s => s.QuarterId == request.QuarterId && s.Month == request.Month);

            var quarter = await _context.Quarters
                .FindAsync(request.QuarterId) ?? throw new EntityNotFoundException(request.QuarterId, nameof(Quarter));

            return _mapper.Map<GetScheduleStatsResponse>(await _statsService.GetScheduleStatsAsync(schedule,
                quarter.DepartamentId, quarter.Year));
        }
    }
}

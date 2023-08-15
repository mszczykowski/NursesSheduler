using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule
{
    internal sealed class GetScheduleQueryHandler : IRequestHandler<GetScheduleRequest, GetScheduleResponse?>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetScheduleQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetScheduleResponse?> Handle(GetScheduleRequest request, CancellationToken cancellationToken)
        {
            var schedule = await _context.Schedules
                .Include(s => s.ScheduleNurses)
                .ThenInclude(n => n.NurseWorkDays)
                .Include(s => s.Quarter)
                .FirstOrDefaultAsync(s => s.Quarter.Year == request.Year 
                    && s.Quarter.DepartamentId == request.DepartamentId 
                    && s.Month == request.Month);

            return schedule is not null ? _mapper.Map<GetScheduleResponse>(schedule) : null;
        }
    }
}

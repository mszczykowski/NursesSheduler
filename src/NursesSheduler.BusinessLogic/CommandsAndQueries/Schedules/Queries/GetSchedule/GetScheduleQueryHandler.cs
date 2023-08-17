using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Services;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule
{
    internal sealed class GetScheduleQueryHandler : IRequestHandler<GetScheduleRequest, GetScheduleResponse?>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ISchedulesService _schedulesService;

        public GetScheduleQueryHandler(IApplicationDbContext context, IMapper mapper, ISchedulesService schedulesService)
        {
            _context = context;
            _mapper = mapper;
            _schedulesService = schedulesService;
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

            if(schedule is not null && !schedule.IsClosed)
            {
                await _schedulesService.SetTimeOffsAsync(request.Year, request.Month, schedule);
            }

            return schedule is not null ? _mapper.Map<GetScheduleResponse>(schedule) : null;
        }
    }
}

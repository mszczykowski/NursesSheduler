using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Exceptions;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule
{
    internal class GetScheduleQueryHandler : IRequestHandler<GetScheduleRequest, GetScheduleResponse>
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

        public async Task<GetScheduleResponse> Handle(GetScheduleRequest request, CancellationToken cancellationToken)
        {
            var schedule = await _context.Schedules
                .Include(s => s.ScheduleNurses)
                .ThenInclude(n => n.NurseWorkDays)
                .FirstOrDefaultAsync(s => s.QuarterId == request.QuarterId && s.Month == request.Month);

            if(schedule is not null)
            {
                return _mapper.Map<GetScheduleResponse>(schedule);
            }

            var quarter = await _context.Quarters
                .FindAsync(request.QuarterId) ?? throw new EntityNotFoundException(request.QuarterId, nameof(Quarter));

            return _mapper.Map<GetScheduleResponse>(await _schedulesService
                .CreateNewScheduleAsync(request.Month, quarter));
        }
    }
}

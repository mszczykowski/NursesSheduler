using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Exceptions;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule
{
    internal class BuildScheduleQueryHandler : IRequestHandler<BuildScheduleRequest, BuildScheduleResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ISchedulesService _schedulesService;

        public BuildScheduleQueryHandler(IApplicationDbContext context, IMapper mapper, ISchedulesService schedulesService)
        {
            _context = context;
            _mapper = mapper;
            _schedulesService = schedulesService;
        }

        public async Task<BuildScheduleResponse> Handle(BuildScheduleRequest request, CancellationToken cancellationToken)
        {
            var quarter = await _context.Quarters
                .FindAsync(request.QuarterId) ?? throw new EntityNotFoundException(request.QuarterId, nameof(Quarter));

            return _mapper.Map<BuildScheduleResponse>(await _schedulesService
                .CreateNewScheduleAsync(request.Month, quarter));
        }
    }
}

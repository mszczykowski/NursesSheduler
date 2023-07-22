using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStatsFromScheduleQuery
{
    internal sealed class GetScheduleStatsFromScheduleQueryHandler : IRequestHandler<GetScheduleStatsFromScheduleRequest,
        GetScheduleStatsFromScheduleResponse>
    {
        private readonly IMapper _mapper;
        private readonly IScheduleStatsService _scheduleStatsService;

        public GetScheduleStatsFromScheduleQueryHandler(IMapper mapper, IScheduleStatsService statsService)
        {
            _mapper = mapper;
            _scheduleStatsService = statsService;
        }

        public async Task<GetScheduleStatsFromScheduleResponse> Handle(GetScheduleStatsFromScheduleRequest request,
            CancellationToken cancellationToken)
        {
            var schedule = _mapper.Map<Schedule>(request.Schedule);

            if (schedule.IsClosed)
            {
                _mapper.Map<GetScheduleStatsFromScheduleResponse>(await _scheduleStatsService.GetScheduleStatsAsync(request.Year, 
                    schedule.Month, request.DepartamentId));
            }

            return _mapper.Map<GetScheduleStatsFromScheduleResponse>(await _scheduleStatsService.GetScheduleStatsAsync(request.Year, 
                request.DepartamentId, schedule));
        }
    }
}

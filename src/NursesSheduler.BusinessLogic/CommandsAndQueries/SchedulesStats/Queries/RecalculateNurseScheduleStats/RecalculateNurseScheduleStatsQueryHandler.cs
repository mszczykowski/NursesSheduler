using AutoMapper;
using MediatR;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.RecalculateNurseScheduleStats
{
    internal sealed class RecalculateNurseScheduleStatsQueryHandler
        : IRequestHandler<RecalculateNurseScheduleStatsRequest, RecalculateNursesScheduleStatsResponse>
    {
        private readonly IMapper _mapper;
        private readonly IScheduleStatsService _scheduleStatsService;

        public RecalculateNurseScheduleStatsQueryHandler(IMapper mapper, IScheduleStatsService scheduleStatsService)
        {
            _mapper = mapper;
            _scheduleStatsService = scheduleStatsService;
        }

        public async Task<RecalculateNursesScheduleStatsResponse> Handle(RecalculateNurseScheduleStatsRequest request,
            CancellationToken cancellationToken)
        {
            var scheduleNurse = _mapper.Map<ScheduleNurse>(request.ScheduleNurse);

            return _mapper.Map<RecalculateNursesScheduleStatsResponse>(await _scheduleStatsService
                .RecalculateNurseScheduleStats(request.Year, request.Month, request.DepartamentId, scheduleNurse));
        }
    }
}

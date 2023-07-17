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
        private readonly IStatsService _statsService;

        public RecalculateNurseScheduleStatsQueryHandler(IMapper mapper, IStatsService statsService)
        {
            _mapper = mapper;
            _statsService = statsService;
        }

        public async Task<RecalculateNursesScheduleStatsResponse> Handle(RecalculateNurseScheduleStatsRequest request,
            CancellationToken cancellationToken)
        {
            var scheduleNurse = _mapper.Map<ScheduleNurse>(request.ScheduleNurse);

            return _mapper.Map<RecalculateNursesScheduleStatsResponse>(await _statsService
                .RecalculateNurseScheduleStats(scheduleNurse, request.DepartamentId, request.Year, request.Month));
        }
    }
}

using AutoMapper;
using MediatR;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.RecalculateNursesScheduleStats
{
    internal sealed class RecalculateNursesScheduleStatsQueryHandler
        : IRequestHandler<RecalculateNursesScheduleStatsRequest, IEnumerable<RecalculateNursesScheduleStatsResponse>>
    {
        private readonly IStatsService _statsService;
        private readonly IMapper _mapper;
        public async Task<IEnumerable<RecalculateNursesScheduleStatsResponse>> Handle(RecalculateNursesScheduleStatsRequest request,
            CancellationToken cancellationToken)
        {
            var schedule = _mapper.Map<Schedule>(request.Schedule);

            return _mapper.Map<IEnumerable<RecalculateNursesScheduleStatsResponse>>(await _statsService
                .GetNurseScheduleStats(schedule, request.DepartamentId, request.Year));
        }
    }
}

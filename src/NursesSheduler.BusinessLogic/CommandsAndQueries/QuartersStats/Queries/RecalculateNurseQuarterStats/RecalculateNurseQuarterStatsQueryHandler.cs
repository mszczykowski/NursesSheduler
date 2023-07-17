using AutoMapper;
using MediatR;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.QuartersStats.Queries.RecalculateNurseQuarterStats
{
    internal sealed class RecalculateNurseQuarterStatsQueryHandler : IRequestHandler<RecalculateNurseQuarterStatsRequest, 
        RecalculateNurseQuarterStatsResponse>
    {
        private readonly IMapper _mapper;
        private readonly IStatsService _statsService;

        public RecalculateNurseQuarterStatsQueryHandler(IMapper mapper, IStatsService statsService)
        {
            _mapper = mapper;
            _statsService = statsService;
        }

        public async Task<RecalculateNurseQuarterStatsResponse> Handle(RecalculateNurseQuarterStatsRequest request,
            CancellationToken cancellationToken)
        {
            var currentScheduleNursesStats = _mapper.Map<NurseStats>(request.CurrentScheduleNurseStats);

            return _mapper.Map<RecalculateNurseQuarterStatsResponse>(await _statsService
                .RecalculateQuarterNurseStatsAsync(currentScheduleNursesStats, request.CurrentYear, request.CurrentMonth,
                    request.DepartamentId));
        }
    }
}

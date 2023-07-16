using AutoMapper;
using MediatR;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.QuartersStats.Queries.GetQuarterStats
{
    internal class GetQuarterStatsQueryHandler : IRequestHandler<GetQuarterStatsRequest, GetQuarterStatsResponse>
    {
        private readonly IMapper _mapper;
        private readonly IStatsService _statsService;

        public GetQuarterStatsQueryHandler(IMapper mapper, IStatsService statsService)
        {
            _mapper = mapper;
            _statsService = statsService;
        }

        public async Task<GetQuarterStatsResponse> Handle(GetQuarterStatsRequest request, 
            CancellationToken cancellationToken)
        {
            var currentScheduleStats = _mapper.Map<ScheduleStats>(request.CurrentScheduleStats);

            return _mapper.Map<GetQuarterStatsResponse>(await _statsService
                .GetQuarterNurseStats(currentScheduleStats, request.CurrentYear, request.CurrentMonth, 
                    request.DepartamentId));
        }
    }
}

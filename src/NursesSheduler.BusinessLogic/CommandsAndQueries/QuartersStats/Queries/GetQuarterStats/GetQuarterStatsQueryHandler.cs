using AutoMapper;
using MediatR;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.QuartersStats.Queries.GetQuarterStats
{
    internal class GetQuarterStatsQueryHandler : IRequestHandler<GetQuarterStatsRequest, GetQuarterStatsResponse>
    {
        private readonly IMapper _mapper;
        private readonly IQuarterStatsService _quarterStatsService;

        public GetQuarterStatsQueryHandler(IMapper mapper, IQuarterStatsService quarterStatsService)
        {
            _mapper = mapper;
            _quarterStatsService = quarterStatsService;
        }

        public async Task<GetQuarterStatsResponse> Handle(GetQuarterStatsRequest request, 
            CancellationToken cancellationToken)
        {
            var currentScheduleStats = _mapper.Map<ScheduleStats>(request.CurrentScheduleStats);

            return _mapper.Map<GetQuarterStatsResponse>(await _quarterStatsService
                .GetQuarterStatsAsync(currentScheduleStats, request.Year, request.Month, request.DepartamentId));
        }
    }
}

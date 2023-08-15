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
        private readonly IQuarterStatsService _quarterStatsService;

        public RecalculateNurseQuarterStatsQueryHandler(IMapper mapper, IQuarterStatsService quarterStatsService)
        {
            _mapper = mapper;
            _quarterStatsService = quarterStatsService;
        }

        public async Task<RecalculateNurseQuarterStatsResponse> Handle(RecalculateNurseQuarterStatsRequest request,
            CancellationToken cancellationToken)
        {
            var currentScheduleNursesStats = _mapper.Map<NurseStats>(request.CurrentScheduleNurseStats);

            return _mapper.Map<RecalculateNurseQuarterStatsResponse>(await _quarterStatsService
                .RecalculateQuarterNurseStatsAsync(currentScheduleNursesStats, request.Year, request.Month,
                    request.DepartamentId));
        }
    }
}

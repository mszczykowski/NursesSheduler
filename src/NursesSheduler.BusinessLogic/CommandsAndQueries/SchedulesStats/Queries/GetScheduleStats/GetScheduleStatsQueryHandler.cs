using AutoMapper;
using MediatR;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure.Providers;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStats
{
    internal class GetScheduleStatsQueryHandler : IRequestHandler<GetScheduleStatsRequest, GetScheduleStatsResponse>
    {
        private readonly IMapper _mapper;
        private readonly IScheduleStatsProvider _scheduleStatsProvider;

        public GetScheduleStatsQueryHandler(IMapper mapper, IScheduleStatsProvider scheduleStatsProvider)
        {
            _mapper = mapper;
            _scheduleStatsProvider = scheduleStatsProvider;
        }

        public async Task<GetScheduleStatsResponse> Handle(GetScheduleStatsRequest request, CancellationToken cancellationToken)
        {
            return _mapper.Map<GetScheduleStatsResponse>(await _scheduleStatsProvider
                .GetCachedDataAsync(_mapper.Map<ScheduleStatsKey>(request)));
        }
    }
}

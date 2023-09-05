using AutoMapper;
using MediatR;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure.Providers;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.NurseStats.Commands.RecalculateNurseStats
{
    internal sealed class RecalculateNurseStatsCommandHandler : IRequestHandler<RecalculateNurseStatsRequest, RecalculateNurseStatsResponse>
    {
        private readonly IMapper _mapper;
        private readonly IScheduleStatsService _scheduleStatsService;
        private readonly IQuarterStatsService _quarterStatsService;
        private readonly IScheduleValidatorService _scheduleValidatorService;
        private readonly IScheduleStatsProvider _scheduleStatsProvider;

        public RecalculateNurseStatsCommandHandler(IMapper mapper, IScheduleStatsService scheduleStatsService,
            IQuarterStatsService quarterStatsService, IScheduleValidatorService scheduleValidatorService,
            IScheduleStatsProvider scheduleStatsProvider)
        {
            _mapper = mapper;
            _scheduleStatsService = scheduleStatsService;
            _quarterStatsService = quarterStatsService;
            _scheduleValidatorService = scheduleValidatorService;
            _scheduleStatsProvider = scheduleStatsProvider;
        }

        public async Task<RecalculateNurseStatsResponse> Handle(RecalculateNurseStatsRequest request,
            CancellationToken cancellationToken)
        {
            var scheduleNurse = _mapper.Map<ScheduleNurse>(request.ScheduleNurse);

            var nurseScheduleStats = await _scheduleStatsService
                .RecalculateNurseScheduleStats(request.Year, request.Month, request.DepartamentId, scheduleNurse);

            var nurseQuarterStats = await _quarterStatsService.RecalculateQuarterNurseStatsAsync(nurseScheduleStats,
                request.Year, request.Month, request.DepartamentId);

            var previousScheduleStats = await _scheduleStatsProvider.GetCachedDataAsync(new ScheduleStatsKey
            {
                DepartamentId = request.DepartamentId,
                Year = request.Month - 1 > 0 ? request.Year : request.Year - 1,
                Month = request.Month - 1 > 0 ? request.Month - 1 : 12,
            });

            var validationErrors = await _scheduleValidatorService.ValidateScheduleNurse(request.TotalWorkTimeInQuarter,
                scheduleNurse, nurseQuarterStats, previousScheduleStats.NursesScheduleStats.FirstOrDefault(n => n.NurseId == scheduleNurse.NurseId),
                request.DepartamentId);

            return new RecalculateNurseStatsResponse
            {
                NursesScheduleStats = _mapper.Map<RecalculateNurseStatsResponse.NursesStatsResponse>(nurseScheduleStats),
                NursesQuarterStats = _mapper.Map<RecalculateNurseStatsResponse.NursesStatsResponse>(nurseQuarterStats),
                ValidationErrors = _mapper
                    .Map<IEnumerable<RecalculateNurseStatsResponse.ScheduleValidationErrorResponse>>(validationErrors),
            };
        }
    }
}

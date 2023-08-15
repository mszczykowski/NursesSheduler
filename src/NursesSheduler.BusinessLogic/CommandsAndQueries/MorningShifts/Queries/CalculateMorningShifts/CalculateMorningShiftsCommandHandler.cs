using AutoMapper;
using MediatR;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure.Providers;
using NursesScheduler.BusinessLogic.Abstractions.Services;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.MorningShifts.Queries.CalculateMorningShifts
{
    internal sealed class CalculateMorningShiftsCommandHandler : IRequestHandler<CalculateMorningShiftsRequest,
        IEnumerable<CalculateMorningShiftsResponse>>
    {
        private readonly IWorkTimeService _workTimeService;
        private readonly IDepartamentSettingsProvider _departamentSettingsManager;
        private readonly IMapper _mapper;

        public CalculateMorningShiftsCommandHandler(IWorkTimeService workTimeService,
            IDepartamentSettingsProvider departamentSettingsManager,
            IMapper mapper)
        {
            _workTimeService = workTimeService;
            _departamentSettingsManager = departamentSettingsManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CalculateMorningShiftsResponse>> Handle(CalculateMorningShiftsRequest request,
            CancellationToken cancellationToken)
        {
            var departamentSettings = await _departamentSettingsManager.GetCachedDataAsync(request.DepartamentId);

            return _mapper.Map<IEnumerable<CalculateMorningShiftsResponse>>(_workTimeService
                .CalculateMorningShifts(request.TimeForMorningShifts, departamentSettings));
        }
    }
}

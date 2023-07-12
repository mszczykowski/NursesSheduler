using AutoMapper;
using MediatR;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure.Providers;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.BusinessLogic.Exceptions;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.MorningShifts.Queries.CalculateMorningShifts
{
    internal sealed class CalculateMorningShiftsCommandHandler : IRequestHandler<CalculateMorningShiftsRequest,
        ICollection<CalculateMorningShiftsResponse>>
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

        public async Task<ICollection<CalculateMorningShiftsResponse>> Handle(CalculateMorningShiftsRequest request,
            CancellationToken cancellationToken)
        {
            var departamentSettings = await _departamentSettingsManager.GetDepartamentSettings(request.DepartamentId);

            if (departamentSettings == null)
                throw new EntityNotFoundException(request.DepartamentId, nameof(DepartamentsSettings));

            return _mapper.Map<ICollection<CalculateMorningShiftsResponse>>(_workTimeService
                .CalculateMorningShifts(request.TimeForMorningShifts, departamentSettings));
        }
    }
}

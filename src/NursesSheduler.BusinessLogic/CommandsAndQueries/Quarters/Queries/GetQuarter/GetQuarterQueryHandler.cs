using AutoMapper;
using MediatR;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure.Providers;
using NursesScheduler.BusinessLogic.Abstractions.Services;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Quarters.Queries.GetQuarter
{
    internal sealed class GetQuarterQueryHandler : IRequestHandler<GetQuarterRequest, GetQuarterResponse?>
    {
        private readonly ICalendarService _calendarService;
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IDepartamentSettingsProvider _departamentSettingsProvider;
        private readonly IMapper _mapper;

        public GetQuarterQueryHandler(ICalendarService calendarService, IApplicationDbContext applicationDbContext, 
            IDepartamentSettingsProvider departamentSettingsProvider, IMapper mapper)
        {
            _calendarService = calendarService;
            _applicationDbContext = applicationDbContext;
            _departamentSettingsProvider = departamentSettingsProvider;
            _mapper = mapper;
        }

        public async Task<GetQuarterResponse?> Handle(GetQuarterRequest request, CancellationToken cancellationToken)
        {
            var departamentSettings = await _departamentSettingsProvider.GetCachedDataAsync(request.DepartamentId);

            var quarterNumber = _calendarService.GetQuarterNumber(request.Month, departamentSettings.FirstQuarterStart);

            var quarter = _applicationDbContext.Quarters
                .FirstOrDefault(q => q.DepartamentId == request.DepartamentId && q.Year == request.Year
                    && q.QuarterNumber == quarterNumber);

            return quarter is not null ? _mapper.Map<GetQuarterResponse>(quarter) : null;
        }
    }
}

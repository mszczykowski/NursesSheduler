using AutoMapper;
using MediatR;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure.Providers;
using NursesScheduler.BusinessLogic.Abstractions.Services;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Days.Queries.GetMonthDays
{
    internal sealed class GetMonthDaysQueryHandler : IRequestHandler<GetMonthDaysRequest, GetMonthDaysResponse>
    {
        private readonly ICalendarService _calendarService;
        private readonly IHolidaysProvider _holidaysProvider;
        private readonly IMapper _mapper;

        public GetMonthDaysQueryHandler(ICalendarService calendarService, IHolidaysProvider holidaysProvider, IMapper mapper)
        {
            _calendarService = calendarService;
            _holidaysProvider = holidaysProvider;
            _mapper = mapper;
        }

        public async Task<GetMonthDaysResponse> Handle(GetMonthDaysRequest request, CancellationToken cancellationToken)
        {
            return new GetMonthDaysResponse
            {
                MonthDays = _mapper.Map<IEnumerable<GetMonthDaysResponse.DayResponse>>(await _calendarService
                    .GetMonthDaysAsync(request.Year, request.Month)),
                HolidaysLoaded = (await _holidaysProvider.GetCachedDataAsync(request.Year)).Any(),
            };
        }
    }
}

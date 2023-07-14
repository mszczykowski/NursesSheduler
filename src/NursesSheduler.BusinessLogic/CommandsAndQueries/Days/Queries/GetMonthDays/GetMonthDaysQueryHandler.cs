using AutoMapper;
using MediatR;
using NursesScheduler.BusinessLogic.Abstractions.Services;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Days.Queries.GetMonthDays
{
    internal sealed class GetMonthDaysQueryHandler : IRequestHandler<GetMonthDaysRequest, 
        IEnumerable<GetMonthDaysResponse>>
    {
        private readonly ICalendarService _calendarService;
        private readonly IMapper _mapper;

        public GetMonthDaysQueryHandler(ICalendarService calendarService, IMapper mapper)
        {
            _calendarService = calendarService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetMonthDaysResponse>> Handle(GetMonthDaysRequest request, 
            CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<GetMonthDaysResponse>>(await _calendarService
                .GetMonthDays(request.Month, request.Year));
        }
    }
}

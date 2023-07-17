using AutoMapper;
using MediatR;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure.Providers;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.BusinessLogic.Exceptions;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Quarters.Commands.AddQuarter
{
    internal sealed class AddQuarterCommandHandler : IRequestHandler<AddQuarterRequest, AddQuarterResponse?>
    {
        private readonly ICalendarService _calendarService;
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IDepartamentSettingsProvider _departamentSettingsProvider;
        private readonly IMapper _mapper;

        public AddQuarterCommandHandler(ICalendarService calendarService, IApplicationDbContext applicationDbContext, 
            IDepartamentSettingsProvider departamentSettingsProvider, IMapper mapper)
        {
            _calendarService = calendarService;
            _applicationDbContext = applicationDbContext;
            _departamentSettingsProvider = departamentSettingsProvider;
            _mapper = mapper;
        }

        public async Task<AddQuarterResponse?> Handle(AddQuarterRequest request,
            CancellationToken cancellationToken)
        {
            var departamentSettings = await _departamentSettingsProvider.GetCachedDataAsync(request.DepartamentId);

            var quarterNumber = _calendarService.GetQuarterNumber(request.Month, departamentSettings.FirstQuarterStart);

            var quarter = _applicationDbContext.Quarters
                .FirstOrDefault(q => q.DepartamentId == request.DepartamentId && q.Year == request.Year
                    && q.QuarterNumber == quarterNumber);

            if(quarter is not null)
            {
                throw new EntityAlreadyExistsException(quarter.QuarterId, nameof(Quarter));
            }

            var newQuarter = new Quarter
            {
                QuarterNumber = quarterNumber,
                Year = request.Year,
                DepartamentId = request.DepartamentId,
            };

            await _applicationDbContext.Quarters.AddAsync(newQuarter);

            var result = await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return result > 0 ? _mapper.Map<AddQuarterResponse>(newQuarter) : null;
        }
    }
}

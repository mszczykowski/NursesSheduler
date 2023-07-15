using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure.Providers;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Quarters.Commands.UpsertQuarter
{
    internal sealed class UpsertQuarterCommandHandler : IRequestHandler<UpsertQuarterRequest, UpsertQuarterResponse>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ICalendarService _calendarService;
        private readonly IDepartamentSettingsProvider _departamentSettingsProvider;
        private readonly IMapper _mapper;
        private readonly IWorkTimeService _workTimeService;
        private readonly IQuarterProvider _quarterProvider;

        public UpsertQuarterCommandHandler(IApplicationDbContext applicationDbContext, ICalendarService calendarService,
            IDepartamentSettingsProvider departamentSettingsProvider, IMapper mapper, IWorkTimeService workTimeService
            IQuarterProvider quarterProvider)
        {
            _applicationDbContext = applicationDbContext;
            _calendarService = calendarService;
            _departamentSettingsProvider = departamentSettingsProvider;
            _mapper = mapper;
            _workTimeService = workTimeService;
            _quarterProvider = quarterProvider;
        }

        public async Task<UpsertQuarterResponse> Handle(UpsertQuarterRequest request,
            CancellationToken cancellationToken)
        {
            var departamentSettings = await _departamentSettingsProvider
                    .GetCachedDataAsync(request.DepartamentId);
            var quarterNumber = _calendarService
                .GetQuarterNumber(request.Month, departamentSettings.FirstQuarterStart);

            var quarter = await _applicationDbContext.Quarters
                    .FirstOrDefaultAsync(q => q.DepartamentId == request.DepartamentId
                        && q.QuarterNumber == quarterNumber);

            if (quarter != null && quarter.IsClosed)
            {
                return _mapper.Map<UpsertQuarterResponse>(quarter);
            }

            if (quarter == null)
            {
                quarter = new Quarter
                {
                    QuarterNumber = quarterNumber,
                    Year = request.Year,
                    DepartamentId = request.DepartamentId,
                };

                _applicationDbContext.Quarters.Add(quarter);
            }

            if (quarter.SettingsVersion != departamentSettings.SettingsVersion ||
                quarter.WorkTimeInQuarterToAssign == TimeSpan.Zero)
            {
                quarter.SettingsVersion = departamentSettings.SettingsVersion;
                quarter.WorkTimeInQuarterToAssign = await _workTimeService
                        .GetTotalWorkingHoursInQuarter(quarterNumber, request.Year, departamentSettings);
                quarter.TimeForMorningShifts = await _workTimeService
                        .GetTimeForMorningShifts(quarterNumber, request.Year, departamentSettings);

                _quarterProvider.InvalidateCache(quarter.QuarterId);
                await _applicationDbContext.SaveChangesAsync(cancellationToken);
            }

            return _mapper.Map<UpsertQuarterResponse>(quarter);
        }
    }
}

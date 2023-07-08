using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure.Providers;
using NursesScheduler.BusinessLogic.Abstractions.Services;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule
{
    internal class GetScheduleQueryHandler : IRequestHandler<GetScheduleRequest, GetScheduleResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ISchedulesService _schedulesService;
        private readonly IDepartamentSettingsProvider _departamentSettingsManager;
        private readonly ICalendarService _calendarService;
        private readonly IWorkTimeService _workTimeService;

        public GetScheduleQueryHandler(IApplicationDbContext context, IMapper mapper, ISchedulesService schedulesService,
            IDepartamentSettingsProvider departamentSettingsManager, ICalendarService calendarService,
            IWorkTimeService workTimeService)
        {
            _context = context;
            _mapper = mapper;
            _schedulesService = schedulesService;
            _departamentSettingsManager = departamentSettingsManager;
            _calendarService = calendarService;
            _workTimeService = workTimeService;
        }
        public async Task<GetScheduleResponse> Handle(GetScheduleRequest request, CancellationToken cancellationToken)
        {
            bool readOnly = false;

            var schedule = await _context.Schedules
                .Include(s => s.Quarter)
                .Include(s => s.ScheduleNurses)
                .FirstOrDefaultAsync(s => s.DepartamentId == request.DepartamentId &&
                    s.Year == request.Year &&
                    s.MonthNumber == request.Month);

            var currentSettings = await _departamentSettingsManager.GetDepartamentSettings(request.DepartamentId);

            if (schedule != null && currentSettings.SettingsVersion != schedule.SettingsVersion)
                readOnly = true;

            if (schedule == null)
                schedule = await _schedulesService.GetNewSchedule(request.Month, request.Year, request.DepartamentId,
                    currentSettings);

            await _schedulesService.SetTimeOffs(schedule, currentSettings);
            await _schedulesService.SetNurseWorkTimes(schedule);

            var response = _mapper.Map<GetScheduleResponse>(schedule);

            var monthDays = await _calendarService.GetMonthDays(request.Month, request.Year);

            response.MonthDays = _mapper.Map<GetScheduleResponse.DayResponse[]>(monthDays);

            response.TimeForMorningShifts = await _workTimeService.GetTimeForMorningShifts(schedule.Quarter.QuarterNumber, 
                schedule.Quarter.QuarterYear, currentSettings);

            return response;
        }
    }
}

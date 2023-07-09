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
            var schedule = await _context.Schedules
                .Include(s => s.ScheduleNurses)
                .ThenInclude(n => n.NurseWorkDays)
                .FirstOrDefaultAsync(s => s.DepartamentId == request.DepartamentId &&
                    s.Year == request.Year &&
                    s.Month == request.Month);

            if(schedule is not null && schedule.IsClosed)
            {
                return _mapper.Map<GetScheduleResponse>(schedule);
            }

            var departamentSettings = await _departamentSettingsManager.GetDepartamentSettings(request.DepartamentId);

            if (schedule is null)
            {
                schedule = await _schedulesService.GetNewSchedule(request.Month, request.Year, request.DepartamentId,
                    departamentSettings);
            }
            else
            {
                await _schedulesService.UpdateScheduleNurses(schedule);
            }

            await _schedulesService.SetTimeOffs(schedule, departamentSettings);
            await _schedulesService.CalculateNurseWorkTimes(schedule);

            return _mapper.Map<GetScheduleResponse>(schedule);
        }
    }
}

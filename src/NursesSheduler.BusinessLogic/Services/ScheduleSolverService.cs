using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure.Providers;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.BusinessLogic.Abstractions.Solver;
using NursesScheduler.BusinessLogic.Solver;
using NursesScheduler.BusinessLogic.Solver.Directors;
using NursesScheduler.BusinessLogic.Solver.Managers;
using NursesScheduler.BusinessLogic.Solver.States;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.Services
{
    internal sealed class ScheduleSolverService : IScheduleSolverService
    {
        private readonly IScheduleStatsService _scheduleStatsService;
        private readonly IQuarterStatsService _quarterStatsService;
        private readonly IDepartamentSettingsProvider _departamentSettingsProvider;
        private readonly ISeedService _seedService;
        private readonly ICalendarService _calendarService;
        private readonly IScheduleStatsProvider _scheduleStatsProvider;
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ISchedulesService _schedulesService;
        private readonly IWorkTimeService _workTimeService;

        public ScheduleSolverService(IScheduleStatsService scheduleStatsService, IQuarterStatsService quarterStatsService,
            IDepartamentSettingsProvider departamentSettingsProvider, ISeedService seedService,
            ICalendarService calendarService, IScheduleStatsProvider scheduleStatsProvider,
            IApplicationDbContext applicationDbContext, ISchedulesService schedulesService,
            IWorkTimeService workTimeService)
        {
            _scheduleStatsService = scheduleStatsService;
            _quarterStatsService = quarterStatsService;
            _departamentSettingsProvider = departamentSettingsProvider;
            _seedService = seedService;
            _calendarService = calendarService;
            _scheduleStatsProvider = scheduleStatsProvider;
            _applicationDbContext = applicationDbContext;
            _schedulesService = schedulesService;
            _workTimeService = workTimeService;
        }

        public async Task<IScheduleSolver> GetScheduleSolver(int year, int departamentId, Schedule currentSchedule)
        {
            var morningShifts = await _applicationDbContext.MorningShifts
                .Where(m => m.QuarterId == currentSchedule.QuarterId)
                .ToListAsync();

            _schedulesService.ResolveMorningShifts(currentSchedule, morningShifts);

            var nurses = await _applicationDbContext.Nurses
                .Where(n => n.DepartamentId == departamentId)
                .ToListAsync();

            var departamentSettings = await _departamentSettingsProvider.GetCachedDataAsync(departamentId);

            var scheduleMonthDays = (await _calendarService.GetNumberedMonthDaysAsync(year, currentSchedule.Month,
                departamentSettings.FirstQuarterStart)).ToArray();

            var constraints = new ConstraintsDirector().GetAllConstraints(departamentSettings, scheduleMonthDays);

            var currentScheduleStats = await _scheduleStatsService.GetScheduleStatsAsync(year, departamentId,
                currentSchedule);

            var previousScheduleStats = await _scheduleStatsProvider.GetCachedDataAsync(new ScheduleStatsKey
            {
                DepartamentId = departamentId,
                Year = currentSchedule.Month - 1 > 0 ? year : year - 1,
                Month = currentSchedule.Month - 1 > 0 ? currentSchedule.Month - 1 : 12,
            });

            var nextScheduleStats = await _scheduleStatsProvider.GetCachedDataAsync(new ScheduleStatsKey
            {
                DepartamentId = departamentId,
                Year = currentSchedule.Month + 1 <= 12 ? year : year + 1,
                Month = currentSchedule.Month + 1 <= 12 ? currentSchedule.Month + 1 : 1,
            });

            var quarterStats = await _quarterStatsService.GetQuarterStatsAsync(currentScheduleStats, year,
                currentSchedule.Month, departamentId);

            var initialNurseStates = new NursesStatesDirector().BuildNurseStats(currentSchedule, quarterStats, previousScheduleStats,
                currentScheduleStats, nextScheduleStats, _workTimeService, nurses);

            var initialSolverState = new SolverState(currentSchedule, scheduleMonthDays.Count(), initialNurseStates);

            var shiftCapacityManager = new ShiftCapacityManager(initialNurseStates, initialSolverState, departamentSettings,
                scheduleMonthDays, currentSchedule, currentScheduleStats, morningShifts);

            return new ScheduleSolver(morningShifts, scheduleMonthDays, constraints, departamentSettings,
                shiftCapacityManager, initialSolverState, _workTimeService);
        }
    }
}

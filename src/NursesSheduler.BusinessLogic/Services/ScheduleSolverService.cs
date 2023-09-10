using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure.Providers;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.BusinessLogic.Abstractions.Solver;
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
        private readonly ICalendarService _calendarService;
        private readonly IScheduleStatsProvider _scheduleStatsProvider;
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ISchedulesService _schedulesService;
        private readonly IWorkTimeService _workTimeService;
        private readonly IScheduleSolver _scheduleSolver;

        public ScheduleSolverService(IScheduleStatsService scheduleStatsService, IQuarterStatsService quarterStatsService,
            IDepartamentSettingsProvider departamentSettingsProvider, ICalendarService calendarService, 
            IScheduleStatsProvider scheduleStatsProvider, IApplicationDbContext applicationDbContext, 
            ISchedulesService schedulesService, IWorkTimeService workTimeService, IScheduleSolver scheduleSolver)
        {
            _scheduleStatsService = scheduleStatsService;
            _quarterStatsService = quarterStatsService;
            _departamentSettingsProvider = departamentSettingsProvider;
            _calendarService = calendarService;
            _scheduleStatsProvider = scheduleStatsProvider;
            _applicationDbContext = applicationDbContext;
            _schedulesService = schedulesService;
            _workTimeService = workTimeService;
            _scheduleSolver = scheduleSolver;
        }

        public async Task<IScheduleSolver> GetScheduleSolver(int year, int departamentId, Schedule currentSchedule,
            CancellationToken cancellationToken)
        {
            var morningShifts = await GetMorningShifts(currentSchedule.QuarterId);
            _schedulesService.ResolveMorningShifts(currentSchedule, morningShifts);

            var nurses = await GetNurses(departamentId);

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

            var initialSolverState = new SolverState(initialNurseStates);

            var shiftCapacityManager = new ShiftCapacityManager(initialNurseStates, initialSolverState, departamentSettings,
                scheduleMonthDays, currentSchedule, currentScheduleStats, morningShifts, quarterStats
                .ShiftsToAssignInMonths[currentScheduleStats.MonthInQuarter - 1]);

            _scheduleSolver.InitialiseSolver(morningShifts, scheduleMonthDays, constraints, departamentSettings,
                shiftCapacityManager, initialSolverState, cancellationToken);

            return _scheduleSolver;
        }

        private async Task<IEnumerable<MorningShift>> GetMorningShifts(int quarterId)
        {
            return await _applicationDbContext.MorningShifts
                .Where(m => m.QuarterId == quarterId)
                .ToListAsync();
        }

        private async Task<IEnumerable<Nurse>> GetNurses(int departamentId)
        {
            return await _applicationDbContext.Nurses
                .Where(n => n.DepartamentId == departamentId)
                .ToListAsync();
        }
    }
}

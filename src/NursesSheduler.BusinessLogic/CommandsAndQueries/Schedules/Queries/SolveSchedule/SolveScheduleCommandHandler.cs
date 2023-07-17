using AutoMapper;
using MediatR;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure.Providers;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.BusinessLogic.Abstractions.Solver.Constraints;
using NursesScheduler.BusinessLogic.Abstractions.Solver.StateManagers;
using NursesScheduler.BusinessLogic.Solver;
using NursesScheduler.BusinessLogic.Solver.Directors;
using NursesScheduler.BusinessLogic.Solver.Managers;
using NursesScheduler.BusinessLogic.Solver.StateManagers;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.ValueObjects;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.SolveSchedule
{
    internal sealed class SolveScheduleCommandHandler : IRequestHandler<SolveScheduleRequest,
        SolveScheduleResponse>
    {
        private readonly IMapper _mapper;
        private readonly IStatsService _statsService;
        private readonly IDepartamentSettingsProvider _departamentSettingsProvider;
        private readonly ISeedService _seedService;
        private readonly ICalendarService _calendarService;
        private readonly IScheduleStatsProvider _scheduleStatsProvider;

        public SolveScheduleCommandHandler(IMapper mapper, IStatsService nurseStatsService, 
            IDepartamentSettingsProvider departamentSettingsProvider, ISeedService seedService, 
            ICalendarService calendarService, IScheduleStatsProvider scheduleStatsProvider)
        {
            _mapper = mapper;
            _statsService = nurseStatsService;
            _departamentSettingsProvider = departamentSettingsProvider;
            _seedService = seedService;
            _calendarService = calendarService;
            _scheduleStatsProvider = scheduleStatsProvider;
        }

        public async Task<SolveScheduleResponse> Handle(SolveScheduleRequest request,
            CancellationToken cancellationToken)
        {
            var currentSchedule = _mapper.Map<Schedule>(request.Schedule);
            var solverSettings = _mapper.Map<SolverSettings>(request.SolverSettings);

            if (!solverSettings.UseOwnSeed)
            {
                solverSettings.GeneratorSeed = _seedService.GetSeed();
            }

            var departamentSettings = await _departamentSettingsProvider.GetCachedDataAsync(request.DepartamentId);

            var constraints = new ConstraintsDirector().GetAllConstraints(departamentSettings);

            var monthDays = await _calendarService.GetNumberedMonthDaysAsync(currentSchedule.Month, request.Year,
                departamentSettings.FirstQuarterStart);


            var currentScheduleStats = await _statsService.GetScheduleStatsAsync(currentSchedule, request.DepartamentId,
                request.Year);
            var previousScheduleStats = await _scheduleStatsProvider.GetCachedDataAsync(new ScheduleStatsKey
            {
                DepartamentId = request.DepartamentId,
                Year = request.Year,
                Month = currentSchedule.Month - 1 > 0 ? currentSchedule.Month - 1 : 12,
            });
            var quarterStats = await _statsService.GetQuarterStatsAsync(currentScheduleStats, request.Year,
                currentSchedule.Month, request.DepartamentId);

            ISolverState result = null;
            for (int i = 0; i < (solverSettings.UseOwnSeed ? 1 : solverSettings.NumberOfRetries); i++)
            {
                result = TrySolveSchedule(currentSchedule, currentScheduleStats, previousScheduleStats, quarterStats, 
                    departamentSettings, constraints, new Random(solverSettings.GeneratorSeed.GetHashCode()), monthDays);

                if (result is not null)
                {
                    break;
                }

                solverSettings.GeneratorSeed = _seedService.GetSeed();
            }

            if (result is not null)
            {
                result.PopulateScheduleFromState(currentSchedule);
            }

            return _mapper.Map<SolveScheduleResponse>(currentSchedule);
        }

        private ISolverState? TrySolveSchedule(Schedule currentSchedule, ScheduleStats currentScheduleStats, 
            ScheduleStats previousScheduleStats, QuarterStats quartersStats, DepartamentSettings departamentSettings, 
            ICollection<IConstraint> constraints, Random random, IEnumerable<DayNumbered> monthDays)
        {
            //var nurseStates = new HashSet<INurseState>();

            //foreach (var quarterStats in nurseQuarterStats)
            //{
            //    nurseStates.Add(new NurseState(quarterStats,
            //        previousSchedule.ScheduleNurses.FirstOrDefault(n => n.NurseId == quarterStats.NurseId),
            //        currentSchedule.ScheduleNurses.First(n => n.NurseId == quarterStats.NurseId)));
            //}

            //var initialSolverState = new SolverState(currentSchedule, monthDays, nurseStates);

            //var solver = new ScheduleSolver(currentSchedule.Quarter.MorningShifts, monthDays, constraints,
            //    departamentSettings);

            //var shiftCapacityManager = new ShiftCapacityManager(departamentSettings, monthDays,
            //        nurseQuarterStats.Count, currentSchedule.WorkTimeInMonth, random);

            //return solver.GenerateSchedule(random, shiftCapacityManager, initialSolverState);

            return null;
        }
    }
}

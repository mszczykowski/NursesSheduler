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

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.SolveSchedule
{
    internal sealed class SolveScheduleCommandHandler : IRequestHandler<SolveScheduleRequest,
        SolveScheduleResponse>
    {
        private readonly IMapper _mapper;
        private readonly ISchedulesService _scheduleService;
        private readonly INurseStatsService _nurseStatsService;
        private readonly IDepartamentSettingsProvider _departamentSettingsProvider;
        private readonly ISeedService _seedService;
        private readonly ICalendarService _calendarService;

        public SolveScheduleCommandHandler(IMapper mapper, ISchedulesService scheduleService,
            INurseStatsService nurseStatsService, IDepartamentSettingsProvider departamentSettingsProvider,
            ISeedService seedService, ICalendarService calendarService)
        {
            _mapper = mapper;
            _scheduleService = scheduleService;
            _nurseStatsService = nurseStatsService;
            _departamentSettingsProvider = departamentSettingsProvider;
            _seedService = seedService;
            _calendarService = calendarService;
        }

        public async Task<SolveScheduleResponse> Handle(SolveScheduleRequest request,
            CancellationToken cancellationToken)
        {
            string generatorSeed;
            ISolverState? result = null;

            var currentSchedule = _mapper.Map<Schedule>(request);
            var previousSchedule = await _scheduleService.GetPreviousSchedule(currentSchedule);

            var departamentSettings = await _departamentSettingsProvider.GetDepartamentSettings(request.DepartamentId);

            var nurseQuarterStats = await _nurseStatsService.GetNurseQuarterStats(currentSchedule, departamentSettings);

            var constraints = new ConstraintsDirector().GetAllConstraints(departamentSettings);

            var monthDays = await _calendarService.GetMonthDays(currentSchedule.Month, currentSchedule.Year,
                departamentSettings.FirstQuarterStart);

            if (request.UseSpecifiedSeed)
            {
                generatorSeed = request.GeneratorSeed;
                request.NumberOfRetries = 1;
            }
            else
            {
                generatorSeed = _seedService.GetSeed();
            }

            for (int i = 0; i < request.NumberOfRetries; i++)
            {
                result = TrySolveSchedule(currentSchedule, previousSchedule, departamentSettings,
                    constraints, nurseQuarterStats, new Random(generatorSeed.GetHashCode()), monthDays);

                if (result is not null)
                {
                    break;
                }

                generatorSeed = _seedService.GetSeed();
            }

            if (result is not null)
            {
                result.PopulateScheduleFromState(currentSchedule);
            }

            return _mapper.Map<SolveScheduleResponse>(currentSchedule);
        }

        private ISolverState? TrySolveSchedule(Schedule currentSchedule, Schedule previousSchedule,
            DepartamentSettings departamentSettings, ICollection<IConstraint> constraints,
            ICollection<NurseQuarterStats> nurseQuarterStats, Random random, Day[] monthDays)
        {
            var nurseStates = new HashSet<INurseState>();

            foreach (var quarterStats in nurseQuarterStats)
            {
                nurseStates.Add(new NurseState(quarterStats,
                    previousSchedule.ScheduleNurses.FirstOrDefault(n => n.NurseId == quarterStats.NurseId),
                    currentSchedule.ScheduleNurses.First(n => n.NurseId == quarterStats.NurseId)));
            }

            var initialSolverState = new SolverState(currentSchedule, monthDays, nurseStates);

            var solver = new ScheduleSolver(currentSchedule.Quarter.MorningShifts, monthDays, constraints,
                departamentSettings);

            var shiftCapacityManager = new ShiftCapacityManager(departamentSettings, monthDays,
                    nurseQuarterStats.Count, currentSchedule.WorkTimeInMonth, random);

            return solver.GenerateSchedule(random, shiftCapacityManager, initialSolverState);
        }
    }
}

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure.Providers;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.BusinessLogic.Abstractions.Solver.Constraints;
using NursesScheduler.BusinessLogic.Abstractions.Solver.States;
using NursesScheduler.BusinessLogic.Solver.Directors;
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
        private readonly IScheduleStatsService _scheduleStatsService;
        private readonly IQuarterStatsService _quarterStatsService;
        private readonly IDepartamentSettingsProvider _departamentSettingsProvider;
        private readonly ISeedService _seedService;
        private readonly ICalendarService _calendarService;
        private readonly IScheduleStatsProvider _scheduleStatsProvider;
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ISchedulesService _schedulesService;

        public async Task<SolveScheduleResponse> Handle(SolveScheduleRequest request,
            CancellationToken cancellationToken)
        {
            var currentSchedule = _mapper.Map<Schedule>(request.Schedule);
            await _schedulesService.ResolveMorningShifts(currentSchedule);

            var solverSettings = _mapper.Map<SolverSettings>(request.SolverSettings);
            
            var nurses = await _applicationDbContext.Nurses
                .Where(n => n.DepartamentId == request.DepartamentId)
                .ToListAsync();

            var departamentSettings = await _departamentSettingsProvider.GetCachedDataAsync(request.DepartamentId);

            var constraints = new ConstraintsDirector().GetAllConstraints(departamentSettings);

            var monthDays = await _calendarService.GetNumberedMonthDaysAsync(currentSchedule.Month, request.Year,
                departamentSettings.FirstQuarterStart);


            var currentScheduleStats = await _scheduleStatsService.GetScheduleStatsAsync(currentSchedule, request.DepartamentId,
                request.Year);
            var previousScheduleStats = await _scheduleStatsProvider.GetCachedDataAsync(new ScheduleStatsKey
            {
                DepartamentId = request.DepartamentId,
                Year = request.Year,
                Month = currentSchedule.Month - 1 > 0 ? currentSchedule.Month - 1 : 12,
            });
            var quarterStats = await _quarterStatsService.GetQuarterStatsAsync(currentScheduleStats, request.Year,
                currentSchedule.Month, request.DepartamentId);

            if (!solverSettings.UseOwnSeed)
            {
                solverSettings.GeneratorSeed = _seedService.GetSeed();
            }

            ISolverState result = null;
            for (int i = 0; i < (solverSettings.UseOwnSeed ? 1 : solverSettings.NumberOfRetries); i++)
            {
                result = TrySolveSchedule(nurses, currentSchedule, currentScheduleStats, previousScheduleStats, quarterStats, 
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

        private ISolverState? TrySolveSchedule(IEnumerable<Nurse> nurses, Schedule currentSchedule, ScheduleStats currentScheduleStats, 
            ScheduleStats previousScheduleStats, QuarterStats quartersStats, DepartamentSettings departamentSettings, 
            ICollection<IConstraint> constraints, Random random, IEnumerable<DayNumbered> monthDays)
        {
            var nurseStatesTeamA = new HashSet<INurseState>();
            var nurseStatesTeamB = new HashSet<INurseState>();

            foreach (var scheduleNurse in currentSchedule.ScheduleNurses)
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

            return null;
        }
    }
}

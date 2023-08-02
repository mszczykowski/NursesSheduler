using AutoMapper;
using MediatR;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.BusinessLogic.Abstractions.Solver;
using NursesScheduler.BusinessLogic.Abstractions.Solver.States;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Enums;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.SolveSchedule
{
    internal sealed class SolveScheduleCommandHandler : IRequestHandler<SolveScheduleRequest,
        SolveScheduleResponse>
    {
        private static Thread thread;

        private readonly IMapper _mapper;
        private readonly IScheduleSolverService _scheduleSolverService;
        private readonly ISeedService _seedService;
        private readonly ISolverLoggerService _solverLoggerService;

        public SolveScheduleCommandHandler(IMapper mapper, IScheduleSolverService scheduleSolverService, 
            ISeedService seedService, ISolverLoggerService solverLoggerService)
        {
            _mapper = mapper;
            _scheduleSolverService = scheduleSolverService;
            _seedService = seedService;
            _solverLoggerService = solverLoggerService;
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

            _solverLoggerService.InitialiseLogger(request.SolverEventsListner);

            var solver = await _scheduleSolverService
                .GetScheduleSolver(request.Year, request.DepartamentId, currentSchedule, cancellationToken);

            var response = new SolveScheduleResponse();

            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(new ThreadStart(() => TryGenerateSchedule(response, solverSettings, solver, 
                    currentSchedule, cancellationToken)));
                thread.Start();
            }

            return response;
        }


        private void TryGenerateSchedule(SolveScheduleResponse response, SolverSettings solverSettings, 
            IScheduleSolver solver, Schedule currentSchedule, CancellationToken cancellationToken)
        {
            response.IsSolverRunning = true;

            ISolverState? result = null;
            for (int i = 0; i < (solverSettings.UseOwnSeed ? 1 : solverSettings.NumberOfRetries); i++)
            {
                _solverLoggerService.LogEvent(SolverEvents.Started);

                result = solver.TrySolveSchedule(new Random(solverSettings.GeneratorSeed.GetHashCode()));

                if (result is not null)
                {
                    _solverLoggerService.LogEvent(SolverEvents.SolutionFound);
                    break;
                }
                if(cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                _solverLoggerService.LogEvent(SolverEvents.SolutionNotFound);

                solverSettings.GeneratorSeed = _seedService.GetSeed();
            }

            if(result is not null)
            {
                result.PopulateScheduleFromState(currentSchedule);

                response.SolverResult = new SolveScheduleResponse.SolverResultResponse
                {
                    ScheduleNurses = _mapper.Map<IEnumerable<SolveScheduleResponse.ScheduleNurseResponse>>(currentSchedule.ScheduleNurses),
                    SolverSettings = _mapper.Map<SolveScheduleResponse.SolverSettingsResponse>(solverSettings),
                };

            }

            response.IsSolverRunning = false;
            _solverLoggerService.LogEvent(SolverEvents.Finished);
        }
    }
}

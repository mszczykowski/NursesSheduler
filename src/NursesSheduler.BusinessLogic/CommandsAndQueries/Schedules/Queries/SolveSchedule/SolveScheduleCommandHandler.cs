using AutoMapper;
using MediatR;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.BusinessLogic.Abstractions.Solver.States;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.SolveSchedule
{
    internal sealed class SolveScheduleCommandHandler : IRequestHandler<SolveScheduleRequest,
        SolveScheduleResponse?>
    {
        private readonly IMapper _mapper;
        private readonly IScheduleSolverService _scheduleSolverService;
        private readonly ISeedService _seedService;

        public SolveScheduleCommandHandler(IMapper mapper, IScheduleSolverService scheduleSolverService, 
            ISeedService seedService)
        {
            _mapper = mapper;
            _scheduleSolverService = scheduleSolverService;
            _seedService = seedService;
        }

        public async Task<SolveScheduleResponse?> Handle(SolveScheduleRequest request,
            CancellationToken cancellationToken)
        {
            var currentSchedule = _mapper.Map<Schedule>(request.Schedule);

            var solverSettings = _mapper.Map<SolverSettings>(request.SolverSettings);

            if (!solverSettings.UseOwnSeed)
            {
                solverSettings.GeneratorSeed = _seedService.GetSeed();
            }

            var solver = await _scheduleSolverService
                .GetScheduleSolver(request.Year, request.DepartamentId, currentSchedule);

            ISolverState? result = null;
            for (int i = 0; i < (solverSettings.UseOwnSeed ? 1 : solverSettings.NumberOfRetries); i++)
            {
                result = solver.TrySolveSchedule(new Random(solverSettings.GeneratorSeed.GetHashCode()));

                if (result is not null)
                {
                    break;
                }

                solverSettings.GeneratorSeed = _seedService.GetSeed();
            }

            if (result is null)
            {
                return null;
            }

            result.PopulateScheduleFromState(currentSchedule);

            return new SolveScheduleResponse
            {
                ScheduleNurses = _mapper
                    .Map<IEnumerable<SolveScheduleResponse.ScheduleNurseResponse>>(currentSchedule.ScheduleNurses),
                SolverSettings = _mapper.Map<SolveScheduleResponse.SolverSettingsResponse>(solverSettings),
            };
        }
    }
}

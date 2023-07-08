using AutoMapper;
using MediatR;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure.Providers;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.BusinessLogic.Abstractions.Solver.StateManagers;
using NursesScheduler.BusinessLogic.Solver.Directors;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Commands.GenerateSchedule
{
    internal sealed class GenerateScheduleCommandHandler : IRequestHandler<GenerateScheduleRequest,
        GenerateScheduleResponse>
    {
        private readonly IMapper _mapper;
        private readonly ISchedulesService _scheduleService;
        private readonly INurseStatsService _nurseStatsService;
        private readonly IDepartamentSettingsProvider _departamentSettingsProvider;
        private readonly ISolverService _solverService;
        private readonly ISeedService _seedService;

        public async Task<GenerateScheduleResponse> Handle(GenerateScheduleRequest request, 
            CancellationToken cancellationToken)
        {
            string generatorSeed;
            ISolverState? result = null;

            var currentSchedule = _mapper.Map<Schedule>(request);
            var previousSchedule = await _scheduleService.GetPreviousSchedule(currentSchedule);

            var departamentSettings = await _departamentSettingsProvider.GetDepartamentSettings(request.DepartamentId);

            var nurseQuarterStats = await _nurseStatsService.GetNurseQuarterStats(currentSchedule, departamentSettings);

            var constraints = new ConstraintsDirector().GetAllConstraints(departamentSettings);

            if(request.UseSpecifiedSeed)
            {
                generatorSeed = request.GeneratorSeed;
                request.NumberOfRetries = 1;
            }
            else
            {
                generatorSeed = _seedService.GetSeed();
            }

            for(int i = 0; i < request.NumberOfRetries; i++)
            {
                result = _solverService.SolveSchedule(currentSchedule, previousSchedule, departamentSettings, 
                    constraints, nurseQuarterStats, new Random(generatorSeed.GetHashCode()));

                if(result is not null)
                {
                    break;
                }

                generatorSeed = _seedService.GetSeed();
            }

            if(result is not null)
            {
                result.PopulateScheduleFromState(currentSchedule);
            }

            return _mapper.Map<GenerateScheduleResponse>(currentSchedule);
        }
    }
}

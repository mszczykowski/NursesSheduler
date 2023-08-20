using AutoMapper;
using MediatR;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.SolverLogs.Queries
{
    internal sealed class GetSolverLogsQueryHandler : IRequestHandler<GetSolverLogsRequest, 
        IEnumerable<GetSolverLogsResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ISolverLoggerService _solverLoggerService;

        public GetSolverLogsQueryHandler(IMapper mapper, ISolverLoggerService solverLoggerService)
        {
            _mapper = mapper;
            _solverLoggerService = solverLoggerService;
        }

        public Task<IEnumerable<GetSolverLogsResponse>> Handle(GetSolverLogsRequest request, 
            CancellationToken cancellationToken)
        {
            bool shouldTryAgain = false;
            do
            {
                try
                {
                    return Task.FromResult(_mapper
                        .Map<IEnumerable<GetSolverLogsResponse>>(new List<SolverLog>(_solverLoggerService.SolverLogs)));
                }
                catch
                {
                    shouldTryAgain = true;
                }
            }
            while (shouldTryAgain);

            return null;
        }
    }
}

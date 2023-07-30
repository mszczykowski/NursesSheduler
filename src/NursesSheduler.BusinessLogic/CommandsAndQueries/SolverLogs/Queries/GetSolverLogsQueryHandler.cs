using AutoMapper;
using MediatR;
using NursesScheduler.BusinessLogic.Abstractions.Services;

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
            return Task.FromResult(_mapper.Map<IEnumerable<GetSolverLogsResponse>>(_solverLoggerService.SolverLogs));
        }
    }
}

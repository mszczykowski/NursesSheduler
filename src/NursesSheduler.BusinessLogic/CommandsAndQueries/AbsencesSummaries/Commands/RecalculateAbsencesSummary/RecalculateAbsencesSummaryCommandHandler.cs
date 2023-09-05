using AutoMapper;
using MediatR;
using NursesScheduler.BusinessLogic.Abstractions.Services;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Commands.RecalculateAbsencesSummary
{
    internal sealed class RecalculateAbsencesSummaryQueryHandler 
        : IRequestHandler<RecalculateAbsencesSummaryRequest, RecalculateAbsencesSummaryResponse>
    {
        private readonly IMapper _mapper;
        private readonly IAbsencesService _absencesService;

        public RecalculateAbsencesSummaryQueryHandler(IMapper mapper, IAbsencesService absencesService)
        {
            _mapper = mapper;
            _absencesService = absencesService;
        }

        public async Task<RecalculateAbsencesSummaryResponse> Handle(RecalculateAbsencesSummaryRequest request,
                                                                                    CancellationToken cancellationToken)
        {
            return _mapper.Map<RecalculateAbsencesSummaryResponse>(await _absencesService
                .RecalculateAbsencesSummary(request.AbsencesSummaryId));
        }
    }
}

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Queries.GetAbsencesSummary
{
    public sealed class GetAbsencesSummaryQueryHandler : IRequestHandler<GetAbsencesSummaryRequest,
                                                                        IEnumerable<GetAbsencesSummaryResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAbsencesSummaryQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAbsencesSummaryResponse>> Handle(GetAbsencesSummaryRequest request,
                                                                                    CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<GetAbsencesSummaryResponse>>(await _context.AbsencesSummaries
                        .Where(y => y.NurseId == request.NurseId).Include(y => y.Absences).ToListAsync());
        }
    }
}

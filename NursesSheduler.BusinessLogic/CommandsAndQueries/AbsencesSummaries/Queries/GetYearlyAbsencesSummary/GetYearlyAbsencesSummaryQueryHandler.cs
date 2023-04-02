using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Queries.GetYearlyAbsencesSummary
{
    public sealed class GetYearlyAbsencesSummaryQueryHandler : IRequestHandler<GetYearlyAbsencesSummaryRequest,
                                                                        ICollection<GetYearlyAbsencesSummaryResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetYearlyAbsencesSummaryQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ICollection<GetYearlyAbsencesSummaryResponse>> Handle(GetYearlyAbsencesSummaryRequest request,
                                                                                    CancellationToken cancellationToken)
        {
            return _mapper.Map<ICollection<GetYearlyAbsencesSummaryResponse>>(await _context.YearlyAbsencesSummaries
                        .Where(y => y.NurseId == request.NurseId).Include(y => y.Absences).ToListAsync());
        }
    }
}

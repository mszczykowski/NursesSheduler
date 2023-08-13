using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Queries.GetAbsencesSummaryByDepartament
{
    internal sealed class GetAbsencesSummaryByDepartamentQueryHandler
        : IRequestHandler<GetAbsencesSummaryByDepartamentRequest, IEnumerable<GetAbsencesSummaryByDepartamentResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAbsencesSummaryByDepartamentQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAbsencesSummaryByDepartamentResponse>> Handle(
            GetAbsencesSummaryByDepartamentRequest request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<GetAbsencesSummaryByDepartamentResponse>>(await
                _context.Nurses
                .Include(n => n.AbsencesSummaries)
                .Where(n => n.DepartamentId == request.DepartamentId)
                .SelectMany(n => n.AbsencesSummaries)
                .ToListAsync());
        }
    }
}

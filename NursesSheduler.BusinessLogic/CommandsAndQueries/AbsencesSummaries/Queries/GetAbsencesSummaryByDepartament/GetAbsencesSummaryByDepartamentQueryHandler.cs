using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Queries.GetAbsencesSummaryByDepartament
{
    internal sealed class GetAbsencesSummaryByDepartamentQueryHandler
                    : IRequestHandler<GetAbsencesSummaryByDepartamentRequest, ICollection<GetAbsencesSummaryByDepartamentResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAbsencesSummaryByDepartamentQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ICollection<GetAbsencesSummaryByDepartamentResponse>> Handle(
            GetAbsencesSummaryByDepartamentRequest request, CancellationToken cancellationToken)
        {
            var x = _mapper.Map<ICollection<GetAbsencesSummaryByDepartamentResponse>>(await _context.Nurses
                        .Where(n => n.DepartamentId == request.DepartamentId)
                        .Include(n => n.AbsencesSummaries)
                        .ToListAsync());

            return x;
        }
    }
}

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Queries
{
    internal sealed class GetAbsencesQueryHandler : IRequestHandler<GetAbsencesRequest, ICollection<GetAbsencesResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAbsencesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ICollection<GetAbsencesResponse>> Handle(GetAbsencesRequest request, 
            CancellationToken cancellationToken)
        {
            return _mapper.Map<ICollection<GetAbsencesResponse>>(await _context.Absences
                        .Where(a => a.AbsencesSummaryId == request.AbsencesSummaryId).ToListAsync());
        }
    }
}

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Nurses.Queries.GetNursesFromDepartament
{
    public class GetNursesFromDepartamentQueryHandler : IRequestHandler<GetNursesFromDepartamentRequest, 
                                                                                List<GetNursesFromDepartamentResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetNursesFromDepartamentQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<GetNursesFromDepartamentResponse>> Handle(GetNursesFromDepartamentRequest request, 
                                                                                    CancellationToken cancellationToken)
        {
            return _mapper.Map<List<GetNursesFromDepartamentResponse>>(await _context.Nurses
                .Where(n => n.DepartamentId == request.DepartamentId && !n.IsDeleted)
                .ToListAsync());
        }
    }
}

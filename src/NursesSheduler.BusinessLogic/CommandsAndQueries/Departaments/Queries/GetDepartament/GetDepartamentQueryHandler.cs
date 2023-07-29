using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Queries.GetDepartament
{
    internal sealed class GetDepartamentQueryHandler : IRequestHandler<GetDepartamentRequest, GetDepartamentResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetDepartamentQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetDepartamentResponse> Handle(GetDepartamentRequest request, 
            CancellationToken cancellationToken)
        {
            return _mapper.Map<GetDepartamentResponse>(await _context.Departaments
                .Include(d => d.DepartamentSettings)
                .FirstOrDefaultAsync(d => d.DepartamentId == request.DepartamentId));
        }
    }
}

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Interfaces.Infrastructure;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Queries.GetDepartament
{
    public class GetDepartamentQueryHandler : IRequestHandler<GetDepartamentRequest, GetDepartamentResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetDepartamentQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetDepartamentResponse> Handle(GetDepartamentRequest request, CancellationToken cancellationToken)
        {
            return _mapper.Map<GetDepartamentResponse>(await _context.Departaments.FirstOrDefaultAsync(d => d.Id == request.Id));
        }
    }
}

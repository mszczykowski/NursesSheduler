using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Nurses.Queries.GetNurse
{
    public sealed class GetNurseQueryHandler : IRequestHandler<GetNurseRequest, GetNurseResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetNurseQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetNurseResponse> Handle(GetNurseRequest request, CancellationToken cancellationToken)
        {
            return _mapper.Map<GetNurseResponse>(await _context.Nurses
                .FirstOrDefaultAsync(n => n.NurseId == request.NurseId));
        }
    }
}

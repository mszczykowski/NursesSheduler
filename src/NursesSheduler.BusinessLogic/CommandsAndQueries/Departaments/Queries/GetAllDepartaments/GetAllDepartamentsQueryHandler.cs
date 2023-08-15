using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Queries.GetAllDepartaments
{
    internal sealed class GetAllDepartamentsQueryHandler : IRequestHandler<GetAllDepartamentsRequest, List<GetAllDepartamentsResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllDepartamentsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<GetAllDepartamentsResponse>> Handle(GetAllDepartamentsRequest request, CancellationToken cancellationToken)
        {
            return _mapper.Map<List<GetAllDepartamentsResponse>>(await _context.Departaments.ToListAsync());
        }
    }
}

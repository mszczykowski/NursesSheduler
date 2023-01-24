using AutoMapper;
using MediatR;
using NursesScheduler.BusinessLogic.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace NursesScheduler.BusinessLogic.Departaments.Queries.GetAllDepartaments
{
    public class GetAllDepartamentsQueryHandler : IRequestHandler<GetAllDepartamentsRequest, List<GetAllDepartamentsResponse>>
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

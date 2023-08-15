using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.MorningShifts.Queries.GetMorningShifts
{
    internal class GetMorningShiftsQueryHandler : IRequestHandler<GetMorningShiftsRequest,
        IEnumerable<GetMorningShiftsResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _applicationDbContext;

        public GetMorningShiftsQueryHandler(IMapper mapper, IApplicationDbContext applicationDbContext)
        {
            _mapper = mapper;
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<GetMorningShiftsResponse>> Handle(GetMorningShiftsRequest request, 
            CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<GetMorningShiftsResponse>>(await _applicationDbContext.MorningShifts
                .Where(m => m.QuarterId == request.QuarterId)
                .ToListAsync());
        }
    }
}

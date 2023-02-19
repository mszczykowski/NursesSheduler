using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Interfaces.Infrastructure;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule
{
    public class GetScheduleQueryHandler : IRequestHandler<GetScheduleRequest, GetScheduleResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetScheduleQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<GetScheduleResponse> Handle(GetScheduleRequest request, CancellationToken cancellationToken)
        {
            var result = await _context.Schedules
                .Where(s => s.DepartamentId == request.DepartamentId && s.Year == request.Year
                                && s.MonthNumber == request.Month)
                .Include(s => s.Shifts)
                .Include(s => s.Holidays)
                .FirstOrDefaultAsync();

            if (result == null) return null;

            return _mapper.Map<GetScheduleResponse>(result);
        }
    }
}

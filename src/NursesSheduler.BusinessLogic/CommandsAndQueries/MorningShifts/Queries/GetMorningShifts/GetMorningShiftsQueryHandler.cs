using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.MorningShifts.Queries.GetMorningShifts
{
    internal class GetMorningShiftsQueryHandler : IRequestHandler<GetMorningShiftsRequest,
        IEnumerable<GetMorningShiftsResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _applicationDbContext;
        public async Task<IEnumerable<GetMorningShiftsResponse>> Handle(GetMorningShiftsRequest request, 
            CancellationToken cancellationToken)
        {
            var morningShifts = await _applicationDbContext.MorningShifts
                .Where(m => m.QuarterId == request.QuarterId)
                .ToListAsync();

            if(!morningShifts.Any())
            {
                for(int i = 0; i <= (int)MorningShiftIndex.R3; i++)
                {
                    morningShifts.Add(new MorningShift
                    {
                        QuarterId = request.QuarterId,
                        Index = (MorningShiftIndex)i,
                        ShiftLength = TimeSpan.Zero,
                    });
                }
            }
            return _mapper.Map<IEnumerable<GetMorningShiftsResponse>>(morningShifts);
        }
    }
}

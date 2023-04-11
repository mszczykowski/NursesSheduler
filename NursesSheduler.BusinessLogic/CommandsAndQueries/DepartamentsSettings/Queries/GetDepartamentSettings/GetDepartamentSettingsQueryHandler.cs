using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.DepartamentsSettings.Queries.GetDepartamentSettings
{
    internal sealed class GetDepartamentSettingsQueryHandler : IRequestHandler<GetDepartamentSettingsRequest, GetDepartamentSettingsResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetDepartamentSettingsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetDepartamentSettingsResponse> Handle(GetDepartamentSettingsRequest request, CancellationToken cancellationToken)
        {
            return _mapper.Map<GetDepartamentSettingsResponse>(await _context.Settings
                .FirstOrDefaultAsync(s => s.DepartamentId == request.DepartamentId));
        }
    }
}

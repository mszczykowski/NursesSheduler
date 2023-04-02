using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.BusinessLogic.Exceptions;
using NursesScheduler.Domain.DomainModels;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Commands.PickDepartament
{
    internal class PickDepartamentCommandHandler : IRequestHandler<PickDepartamentRequest, PickDepartamentResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAbsencesService _absencesService;

        public PickDepartamentCommandHandler(IApplicationDbContext context, IMapper mapper, IAbsencesService absencesService)
        {
            _context = context;
            _mapper = mapper;
            _absencesService = absencesService;
        }

        public async Task<PickDepartamentResponse> Handle(PickDepartamentRequest request, CancellationToken cancellationToken)
        {
            var departament = await _context.Departaments
                                            .FirstOrDefaultAsync(d => d.DepartamentId == request.DepartamentId);

            if (departament == null)
                throw new EntityNotFoundException(request.DepartamentId, nameof(Departament));

            await _absencesService.InitializeDepartamentAbsencesSummary(departament, cancellationToken);

            return _mapper.Map<PickDepartamentResponse>(departament);
        }
    }
}

using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.BusinessLogic.Exceptions;
using NursesScheduler.Domain.DomainModels;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Nurses.Commands.AddNurse
{
    internal sealed class AddNurseCommandHandler : IRequestHandler<AddNurseRequest, AddNurseResponse>
    {
        private readonly IMapper _mapper;
        private readonly IValidator<Nurse> _validator;
        private readonly IApplicationDbContext _context;
        private readonly IAbsencesService _absencesService;

        public AddNurseCommandHandler(IMapper mapper, IValidator<Nurse> validator, IApplicationDbContext context,
             IAbsencesService absencesService)
        {
            _validator = validator;
            _mapper = mapper;
            _context = context;
            _absencesService = absencesService;
        }

        public async Task<AddNurseResponse> Handle(AddNurseRequest request, CancellationToken cancellationToken)
        {
            var nurse = _mapper.Map<Nurse>(request);
            var departamet = _context.Departaments.Include(d => d.Nurses).FirstOrDefault(d => d.DepartamentId == request.DepartamentId);

            if(departamet == null)
            {
                throw new EntityNotFoundException(request.DepartamentId, nameof(Departament));
            }

            var validationResult = await _validator.ValidateAsync(nurse);
            if (!validationResult.IsValid) 
                throw new ValidationException(validationResult.Errors);

            nurse.IsDeleted = false;

            _absencesService.InitializeNewNurseAbsencesSummaries(nurse, departamet);

            departamet.Nurses.Add(nurse);

            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0 ? _mapper.Map<AddNurseResponse>(nurse) : null;
        }
    }
}

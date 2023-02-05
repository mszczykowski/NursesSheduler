using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Interfaces.Infrastructure;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Exceptions;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Commands.EditDepartament
{
    public class EditDepartamentCommandHandler : IRequestHandler<EditDepartamentRequest, EditDepartamentResponse>
    {
        private readonly IMapper _mapper;
        private readonly IValidator<Departament> _validator;
        private readonly IApplicationDbContext _context;

        public EditDepartamentCommandHandler(IMapper mapper, IValidator<Departament> validator, IApplicationDbContext context)
        {
            _mapper = mapper;
            _validator = validator;
            _context = context;
        }

        public async Task<EditDepartamentResponse> Handle(EditDepartamentRequest request, CancellationToken cancellationToken)
        {
            var modifiedDepartament = _mapper.Map<Departament>(request);

            var validationResult = await _validator.ValidateAsync(modifiedDepartament);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            var originalDepartament = await _context.Departaments.FirstOrDefaultAsync(d => d.Id == request.Id) 
                ?? throw new EntityNotFoundException(request.Id, nameof(Departament));


            originalDepartament.Name = modifiedDepartament.Name;

            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0 ? _mapper.Map<EditDepartamentResponse>(originalDepartament) : null;
        }
    }
}

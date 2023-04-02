using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Exceptions;
using NursesScheduler.Domain.DomainModels;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Nurses.Commands.EditNurse
{
    internal sealed class EditNurseCommandHandler : IRequestHandler<EditNurseRequest, EditNurseResponse>
    {
        private readonly IMapper _mapper;
        private readonly IValidator<Nurse> _validator;
        private readonly IApplicationDbContext _context;

        public EditNurseCommandHandler(IMapper mapper, IValidator<Nurse> validator, IApplicationDbContext context)
        {
            _mapper = mapper;
            _validator = validator;
            _context = context;
        }

        public async Task<EditNurseResponse> Handle(EditNurseRequest request, CancellationToken cancellationToken)
        {
            var modifiedNurse = _mapper.Map<Nurse>(request);

            var validationResult = await _validator.ValidateAsync(modifiedNurse);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            var originalNurse = await _context.Nurses.FirstOrDefaultAsync(n => n.NurseId == request.NurseId)
                ?? throw new EntityNotFoundException(request.DepartamentId, nameof(Departament));

            _context.Entry(originalNurse).CurrentValues.SetValues(modifiedNurse);

            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0 ? _mapper.Map<EditNurseResponse>(originalNurse) : null;
        }
    }
}

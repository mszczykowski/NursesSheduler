using AutoMapper;
using FluentValidation;
using MediatR;
using NursesScheduler.BusinessLogic.Interfaces.Infrastructure;
using NursesScheduler.Domain.DomainModels;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Nurses.Commands.AddNurse
{
    public sealed class AddNurseCommandHandler : IRequestHandler<AddNurseRequest, AddNurseResponse>
    {
        private readonly IMapper _mapper;
        private readonly IValidator<Nurse> _validator;
        private readonly IApplicationDbContext _context;

        public AddNurseCommandHandler(IMapper mapper, IValidator<Nurse> validator, IApplicationDbContext context)
        {
            _validator = validator;
            _mapper = mapper;
            _context = context;
        }

        public async Task<AddNurseResponse> Handle(AddNurseRequest request, CancellationToken cancellationToken)
        {
            var nurse = _mapper.Map<Nurse>(request);

            var validationResult = await _validator.ValidateAsync(nurse);
            if (!validationResult.IsValid) 
                throw new ValidationException(validationResult.Errors);

            nurse.IsDeleted = false;

            await _context.Nurses.AddAsync(nurse);

            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0 ? _mapper.Map<AddNurseResponse>(nurse) : null;
        }
    }
}

using AutoMapper;
using FluentValidation;
using MediatR;
using NursesScheduler.BusinessLogic.Interfaces.Infrastructure;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Departaments.Commands.CreateDepartament
{
    public class CreateDepartamentCommandHandler : IRequestHandler<CreateDepartamentRequest, CreateDepartamentResponse>
    {
        private readonly IMapper _mapper;
        private readonly IValidator<CreateDepartamentRequest> _validator;
        private readonly IApplicationDbContext _context;

        public CreateDepartamentCommandHandler(IMapper mapper, IValidator<CreateDepartamentRequest> validator,
            IApplicationDbContext context)
        {
            _validator = validator;
            _mapper = mapper;
            _context = context;
        }

        public async Task<CreateDepartamentResponse> Handle(CreateDepartamentRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            var departamnt = _mapper.Map<Departament>(request);

            await _context.Departaments.AddAsync(departamnt);

            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0 ? _mapper.Map<CreateDepartamentResponse>(departamnt) : null;
        }
    }
}

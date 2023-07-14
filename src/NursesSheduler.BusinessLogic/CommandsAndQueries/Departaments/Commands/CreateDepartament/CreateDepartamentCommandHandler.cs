using AutoMapper;
using FluentValidation;
using MediatR;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Commands.CreateDepartament
{
    internal class CreateDepartamentCommandHandler : IRequestHandler<CreateDepartamentRequest, CreateDepartamentResponse>
    {
        private readonly IMapper _mapper;
        private readonly IValidator<Departament> _validator;
        private readonly IApplicationDbContext _context;
        private readonly ICurrentDateService _currentDateService;

        public CreateDepartamentCommandHandler(IMapper mapper, IValidator<Departament> validator,
            IApplicationDbContext context, ICurrentDateService currentDateService)
        {
            _validator = validator;
            _mapper = mapper;
            _context = context;
            _currentDateService = currentDateService;
        }

        public async Task<CreateDepartamentResponse> Handle(CreateDepartamentRequest request, 
            CancellationToken cancellationToken)
        {
            var departament = _mapper.Map<Departament>(request);

            var validationResult = await _validator.ValidateAsync(departament);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            departament.CreationYear = _currentDateService.GetCurrentDate().Year;
            departament.DepartamentSettings = new DepartamentSettings();

            await _context.Departaments.AddAsync(departament);

            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0 ? _mapper.Map<CreateDepartamentResponse>(departament) : null;
        }
    }
}

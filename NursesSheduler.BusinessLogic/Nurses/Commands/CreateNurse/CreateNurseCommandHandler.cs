using AutoMapper;
using FluentValidation;
using MediatR;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Persistance.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NursesScheduler.BusinessLogic.Nurses.Commands.CreateNurse
{
    public class CreateNurseCommandHandler : IRequestHandler<CreateNurseRequest, CreateNurseResponse>
    {
        private readonly IMapper _mapper;
        private readonly IValidator<CreateNurseRequest> _validator;
        private readonly IApplicationDbContext _context;

        public CreateNurseCommandHandler(IMapper mapper, IValidator<CreateNurseRequest> validator, IApplicationDbContext context)
        {
            _validator = validator;
            _mapper = mapper;
            _context = context;
        }

        public async Task<CreateNurseResponse> Handle(CreateNurseRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            var nurse = _mapper.Map<Nurse>(request);

            await _context.Nurses.AddAsync(nurse);

            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0 ? _mapper.Map<CreateNurseResponse>(nurse) : null;
        }
    }
}

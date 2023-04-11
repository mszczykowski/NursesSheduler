using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Exceptions;
using NursesScheduler.Domain.DomainModels;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.DepartamentsSettings.Commands.EditDepartamentSettings
{
    internal sealed class EditDepartamentSettingsCommandHandler : IRequestHandler<EditDepartamentSettingsRequest,
                                                                                        EditDepartamentSettingsResponse>
    {
        private readonly IMapper _mapper;
        private readonly IValidator<DepartamentSettings> _validator;
        private readonly IApplicationDbContext _context;

        public EditDepartamentSettingsCommandHandler(IMapper mapper, IValidator<DepartamentSettings> validator,
                                                                                        IApplicationDbContext context)
        {
            _mapper = mapper;
            _validator = validator;
            _context = context;
        }

        public async Task<EditDepartamentSettingsResponse> Handle(EditDepartamentSettingsRequest request,
                                                                                    CancellationToken cancellationToken)
        {
            var modifiedSettings = _mapper.Map<DepartamentSettings>(request);

            var validationResult = await _validator.ValidateAsync(modifiedSettings);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            var originalSettings = await _context.Settings
                .FirstOrDefaultAsync(s => s.DepartamentSettingsId == request.DepartamentSettingsId)
                ?? throw new EntityNotFoundException(request.DepartamentSettingsId, nameof(DepartamentSettings));

            _context.Entry(originalSettings).CurrentValues.SetValues(modifiedSettings);

            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0 ? _mapper.Map<EditDepartamentSettingsResponse>(originalSettings) : null;
        }
    }
}

using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.CacheManagers;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Exceptions;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.DepartamentsSettings.Commands.EditDepartamentSettings
{
    internal sealed class EditDepartamentSettingsCommandHandler : IRequestHandler<EditDepartamentSettingsRequest,
                                                                                        EditDepartamentSettingsResponse>
    {
        private readonly IMapper _mapper;
        private readonly IValidator<DepartamentSettings> _validator;
        private readonly IApplicationDbContext _context;
        private readonly IDepartamentSettingsManager _departamentSettingsManager;

        public EditDepartamentSettingsCommandHandler(IMapper mapper, IValidator<DepartamentSettings> validator,
                                  IApplicationDbContext context, IDepartamentSettingsManager departamentSettingsManager)
        {
            _mapper = mapper;
            _validator = validator;
            _context = context;
            _departamentSettingsManager = departamentSettingsManager;
        }

        public async Task<EditDepartamentSettingsResponse> Handle(EditDepartamentSettingsRequest request,
                                                                                    CancellationToken cancellationToken)
        {
            var newSettings = _mapper.Map<DepartamentSettings>(request);

            var validationResult = await _validator.ValidateAsync(newSettings);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            var oldSettings = await _context.DepartamentSettings
                .FirstOrDefaultAsync(s => s.DepartamentSettingsId == request.DepartamentSettingsId)
                ?? throw new EntityNotFoundException(request.DepartamentSettingsId, nameof(DepartamentSettings));

            if (oldSettings.Equals(newSettings))
                return _mapper.Map<EditDepartamentSettingsResponse>(oldSettings);

            oldSettings.SettingsVersion++;

            _context.Entry(oldSettings).CurrentValues.SetValues(request);

            var result = await _context.SaveChangesAsync(cancellationToken);

            _departamentSettingsManager.InvalidateCache(oldSettings.DepartamentId);

            return result > 0 ? _mapper.Map<EditDepartamentSettingsResponse>(oldSettings) : null;
        }
    }
}

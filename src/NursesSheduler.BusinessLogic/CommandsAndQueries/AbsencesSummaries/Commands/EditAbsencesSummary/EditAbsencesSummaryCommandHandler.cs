using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Exceptions;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Commands.EditAbsencesSummary
{
    internal sealed class EditAbsencesSummaryCommandHandler : IRequestHandler<EditAbsencesSummaryRequest, 
                                                                                            EditAbsencesSummaryResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<AbsencesSummary> _validator;

        public EditAbsencesSummaryCommandHandler(IApplicationDbContext context, IMapper mapper, 
                                                                                 IValidator<AbsencesSummary> validator)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<EditAbsencesSummaryResponse> Handle(EditAbsencesSummaryRequest request,
                                                                                    CancellationToken cancellationToken)
        {
            var modifiedSummary = _mapper.Map<AbsencesSummary>(request);

            var validationResult = await _validator.ValidateAsync(modifiedSummary);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var originalSummary = await _context.AbsencesSummaries
                .Include(s => s.Nurse)
                .FirstOrDefaultAsync(s => s.AbsencesSummaryId == request.AbsencesSummaryId)
                ?? throw new EntityNotFoundException(request.AbsencesSummaryId, nameof(AbsencesSummary));

            _context.Entry(originalSummary).CurrentValues.SetValues(request);

            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0 ? _mapper.Map<EditAbsencesSummaryResponse>(originalSummary) : null;
        }
    }
}

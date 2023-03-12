using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Exceptions;
using NursesScheduler.BusinessLogic.Interfaces.Infrastructure;
using NursesScheduler.BusinessLogic.Veryfication;
using NursesScheduler.Domain.DomainModels;
using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Commands.EditAbsence
{
    internal sealed class EditAbsenceCommandHandler : IRequestHandler<EditAbsenceRequest, EditAbsenceResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<Absence> _validator;

        public EditAbsenceCommandHandler(IApplicationDbContext context, IMapper mapper, IValidator<Absence> validator)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<EditAbsenceResponse> Handle(EditAbsenceRequest request, CancellationToken cancellationToken)
        {
            var yearlyAbsenceSummary = await _context.YearlyAbsences.Include(y => y.Absences)
                .FirstOrDefaultAsync(y => y.YearlyAbsencesSummaryId == request.YearlyAbsencesSummaryId)
                ?? throw new EntityNotFoundException(request.YearlyAbsencesSummaryId, nameof(YearlyAbsencesSummary));

            var originalAbsence = yearlyAbsenceSummary.Absences.FirstOrDefault(a => a.AbsenceId == request.AbsenceId) 
                ?? throw new EntityNotFoundException(request.AbsenceId, nameof(Absence));

            var modifiedAbsence = _mapper.Map<Absence>(request);

            var validationResult = await _validator.ValidateAsync(modifiedAbsence);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var absenceVeryficationResult = AbsenceVeryficator.VerifyAbsence(yearlyAbsenceSummary, modifiedAbsence);

            if (absenceVeryficationResult != AbsenceVeryficationResult.Valid)
                return new EditAbsenceResponse
                {
                    VeryficationResult = absenceVeryficationResult
                };

            _context.Entry(originalAbsence).CurrentValues.SetValues(modifiedAbsence);

            yearlyAbsenceSummary.Absences.Add(modifiedAbsence);

            var result = await _context.SaveChangesAsync(cancellationToken);

            var absenceResponse = _mapper.Map<EditAbsenceResponse>(modifiedAbsence);
            absenceResponse.VeryficationResult = absenceVeryficationResult;

            return result > 0 ? absenceResponse : null;
        }
    }
}

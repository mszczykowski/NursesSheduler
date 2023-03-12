using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Exceptions;
using NursesScheduler.BusinessLogic.Interfaces.Infrastructure;
using NursesScheduler.BusinessLogic.Veryfication;
using NursesScheduler.Domain.DomainModels;
using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Commands.AddAbsence
{
    internal sealed class AddAbsenceCommandHandler : IRequestHandler<AddAbsenceRequest, AddAbsenceResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<Absence> _validator;

        public AddAbsenceCommandHandler(IApplicationDbContext context, IMapper mapper, IValidator<Absence> validator)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<AddAbsenceResponse> Handle(AddAbsenceRequest request, CancellationToken cancellationToken)
        {
            var yearlyAbsenceSummary = await _context.YearlyAbsences.Include(y => y.Absences)
                .FirstOrDefaultAsync(y => y.YearlyAbsencesSummaryId == request.YearlyAbsencesSummaryId)
                ?? throw new EntityNotFoundException(request.YearlyAbsencesSummaryId, nameof(YearlyAbsencesSummary));

            var absence = _mapper.Map<Absence>(request);

            var validationResult = await _validator.ValidateAsync(absence);
            if (!validationResult.IsValid) 
                throw new ValidationException(validationResult.Errors);

            var absenceVeryficationResult = AbsenceVeryficator.VerifyAbsence(yearlyAbsenceSummary, absence);

            if (absenceVeryficationResult != AbsenceVeryficationResult.Valid)
                return new AddAbsenceResponse
                {
                    VeryficationResult = absenceVeryficationResult
                };

            yearlyAbsenceSummary.Absences.Add(absence);

            var result = await _context.SaveChangesAsync(cancellationToken);

            var absenceResponse = _mapper.Map<AddAbsenceResponse>(absence);
            absenceResponse.VeryficationResult = absenceVeryficationResult;

            return result > 0 ? absenceResponse : null;
        }
    }
}

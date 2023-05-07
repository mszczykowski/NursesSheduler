using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.BusinessLogic.Exceptions;
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
        private readonly IWorkTimeService _workTimeService;
        private readonly IAbsencesService _absencesService;

        public EditAbsenceCommandHandler(IApplicationDbContext context, IMapper mapper, IValidator<Absence> validator,
                                                    IWorkTimeService workTimeService, IAbsencesService absencesService)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
            _workTimeService = workTimeService;
            _absencesService = absencesService;
        }

        public async Task<EditAbsenceResponse> Handle(EditAbsenceRequest request, CancellationToken cancellationToken)
        {
            var absencesSummary = await _context.AbsencesSummaries.Include(y => y.Absences)
                .FirstOrDefaultAsync(y => y.AbsencesSummaryId == request.AbsencesSummaryId)
                ?? throw new EntityNotFoundException(request.AbsencesSummaryId, nameof(AbsencesSummary));

            var originalAbsence = absencesSummary.Absences.FirstOrDefault(a => a.AbsenceId == request.AbsenceId) 
                ?? throw new EntityNotFoundException(request.AbsenceId, nameof(Absence));

            var modifiedAbsence = _mapper.Map<Absence>(request);

            var validationResult = await _validator.ValidateAsync(modifiedAbsence);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var absenceVeryficationResult = AbsenceVeryficator.VerifyAbsence(absencesSummary, modifiedAbsence);

            if (absenceVeryficationResult != AbsenceVeryficationResult.Valid)
                return new EditAbsenceResponse
                {
                    VeryficationResult = absenceVeryficationResult
                };

            modifiedAbsence.WorkingHoursToAssign = await _workTimeService
                .GetTotalWorkingHoursFromTo(modifiedAbsence.From, modifiedAbsence.To, null);
            modifiedAbsence.AssignedWorkingHours = await _absencesService
                .CalculateAbsenceAssignedWorkingTime(modifiedAbsence);

            _context.Entry(originalAbsence).CurrentValues.SetValues(modifiedAbsence);

            absencesSummary.Absences.Add(modifiedAbsence);
            absencesSummary.PTOTimeUsed = absencesSummary.PTOTimeUsed - originalAbsence.AssignedWorkingHours
                                                    + modifiedAbsence.AssignedWorkingHours;

            var result = await _context.SaveChangesAsync(cancellationToken);

            var absenceResponse = _mapper.Map<EditAbsenceResponse>(modifiedAbsence);
            absenceResponse.VeryficationResult = absenceVeryficationResult;

            return result > 0 ? absenceResponse : null;
        }
    }
}

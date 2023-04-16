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

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Commands.AddAbsence
{
    internal sealed class AddAbsenceCommandHandler : IRequestHandler<AddAbsenceRequest, AddAbsenceResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<Absence> _validator;
        private readonly IWorkTimeService _workTimeService;
        private readonly IAbsencesService _absencesService;

        public AddAbsenceCommandHandler(IApplicationDbContext context, IMapper mapper, IValidator<Absence> validator, 
                                                    IWorkTimeService workTimeService, IAbsencesService absencesService)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
            _workTimeService = workTimeService;
            _absencesService = absencesService;
        }

        public async Task<AddAbsenceResponse> Handle(AddAbsenceRequest request, CancellationToken cancellationToken)
        {
            var absencesSummary = await _context.AbsencesSummaries.Include(y => y.Absences)
                .FirstOrDefaultAsync(y => y.AbsencesSummaryId == request.AbsencesSummaryId)
                ?? throw new EntityNotFoundException(request.AbsencesSummaryId, nameof(AbsencesSummary));

            var absence = _mapper.Map<Absence>(request);

            var validationResult = await _validator.ValidateAsync(absence);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var absenceVeryficationResult = AbsenceVeryficator.VerifyAbsence(absencesSummary, absence);

            if (absenceVeryficationResult != AbsenceVeryficationResult.Valid)
                return new AddAbsenceResponse
                {
                    VeryficationResult = absenceVeryficationResult
                };

            absence.WorkingHoursToAssign = await _workTimeService.GetTotalWorkingHours(absence.From, absence.To);
            absence.AssignedWorkingHours = await _absencesService.CalculateAbsenceAssignedWorkingTime(absence);

            absencesSummary.Absences.Add(absence);

            var result = await _context.SaveChangesAsync(cancellationToken);

            var absenceResponse = _mapper.Map<AddAbsenceResponse>(absence);
            absenceResponse.VeryficationResult = absenceVeryficationResult;

            return result > 0 ? absenceResponse : null;
        }
    }
}

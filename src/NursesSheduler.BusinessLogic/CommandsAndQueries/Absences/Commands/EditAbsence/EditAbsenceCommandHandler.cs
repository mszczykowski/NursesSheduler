using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure.Providers;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Enums;
using NursesScheduler.Domain.Exceptions;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Commands.EditAbsence
{
    internal sealed class EditAbsenceCommandHandler : IRequestHandler<EditAbsenceRequest, EditAbsenceResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<EditAbsenceRequest> _validator;
        private readonly IWorkTimeService _workTimeService;
        private readonly IAbsencesService _absencesService;
        private readonly ICalendarService _calendarService;
        private readonly IDepartamentSettingsProvider _departamentSettingsProvider;

        public EditAbsenceCommandHandler(IApplicationDbContext context, IMapper mapper,
            IValidator<EditAbsenceRequest> validator, IWorkTimeService workTimeService, IAbsencesService absencesService,
            ICalendarService calendarService, IDepartamentSettingsProvider departamentSettingsProvider)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
            _workTimeService = workTimeService;
            _absencesService = absencesService;
            _calendarService = calendarService;
            _departamentSettingsProvider = departamentSettingsProvider;
        }

        public async Task<EditAbsenceResponse> Handle(EditAbsenceRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var originalAbsence = await _context.Absences
                .FirstOrDefaultAsync(a => a.AbsenceId == request.AbsenceId)
                ?? throw new EntityNotFoundException(request.AbsenceId, nameof(AbsencesSummary));

            var absencesSummary = await _context.AbsencesSummaries
                .Include(s => s.Absences)
                .FirstOrDefaultAsync(s => s.AbsencesSummaryId == originalAbsence.AbsencesSummaryId)
                ?? throw new EntityNotFoundException(originalAbsence.AbsencesSummaryId, nameof(AbsencesSummary));

            var modifiedAbsence = _absencesService.GetAbsencesFromDates(request.From, request.To,
                originalAbsence.AbsencesSummaryId, request.Type).First();

            var veryficationResult = await _absencesService.VerifyAbsence(absencesSummary, modifiedAbsence);
            if (veryficationResult != AbsenceVeryficationResult.Valid)
            {
                return new EditAbsenceResponse
                {
                    VeryficationResult = veryficationResult,
                };
            }

            var departamentSettings = await _departamentSettingsProvider.GetCachedDataAsync(absencesSummary.Nurse.DepartamentId);

            var days = await _calendarService.GetDaysFromDayNumbersAsync(absencesSummary.Year, modifiedAbsence.Month,
                    modifiedAbsence.Days);
            modifiedAbsence.WorkTimeToAssign = _workTimeService.GetWorkTimeFromDays(days, departamentSettings);
            modifiedAbsence.AbsencesSummaryId = absencesSummary.AbsencesSummaryId;


            originalAbsence.Days = modifiedAbsence.Days;
            originalAbsence.Type = modifiedAbsence.Type;
            originalAbsence.WorkTimeToAssign = modifiedAbsence.WorkTimeToAssign;

            var result = await _context.SaveChangesAsync(cancellationToken);

            return new EditAbsenceResponse
            {
                VeryficationResult = veryficationResult,
                Absence = result > 0 ? _mapper.Map<EditAbsenceResponse.AbsenceResponse>(originalAbsence) : null,
            };
        }
    }
}

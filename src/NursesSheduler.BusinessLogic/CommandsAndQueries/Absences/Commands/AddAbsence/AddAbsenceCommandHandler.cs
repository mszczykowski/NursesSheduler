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

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Commands.AddAbsence
{
    internal sealed class AddAbsenceCommandHandler : IRequestHandler<AddAbsenceRequest, AddAbsenceResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<AddAbsenceRequest> _validator;
        private readonly IWorkTimeServiceLegacy _workTimeService;
        private readonly IAbsencesService _absencesService;
        private readonly ICalendarService _calendarService;
        private readonly IDepartamentSettingsProvider _settingsManager;

        public AddAbsenceCommandHandler(IApplicationDbContext context, IMapper mapper, 
            IValidator<AddAbsenceRequest> validator, IWorkTimeServiceLegacy workTimeService, IAbsencesService absencesService,
            ICalendarService calendarService, IDepartamentSettingsProvider settingsManager)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
            _workTimeService = workTimeService;
            _absencesService = absencesService;
            _calendarService = calendarService;
            _settingsManager = settingsManager;
        }

        public async Task<AddAbsenceResponse> Handle(AddAbsenceRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var absences = _absencesService.GetAbsencesFromDates(request.From, request.To, 
                request.AbsencesSummaryId, request.Type);

            var absencesSummary = await _context.AbsencesSummaries
                .Include(y => y.Absences)
                .Include(y => y.Nurse)
                .FirstOrDefaultAsync(y => y.AbsencesSummaryId == request.AbsencesSummaryId)
                ?? throw new EntityNotFoundException(request.AbsencesSummaryId, nameof(AbsencesSummary));

            var veryficationResult = AbsenceVeryficationResult.Valid;
            foreach (var absence in absences)
            {
                veryficationResult = await _absencesService.VerifyAbsence(absencesSummary, absence);

                if(veryficationResult != AbsenceVeryficationResult.Valid)
                {
                    break;
                }
            }

            if (veryficationResult != AbsenceVeryficationResult.Valid)
            {
                return new AddAbsenceResponse(veryficationResult);
            }

            var departamentSettings = await _settingsManager.GetCachedDataAsync(absencesSummary.Nurse.DepartamentId);

            foreach (var absence in absences)
            {
                var days = await _calendarService.GetDaysFromDayNumbersAsync(absencesSummary.Year, absence.Month, 
                    absence.Days);
                absence.WorkTimeToAssign = _workTimeService.GetWorkTimeFromDays(days, departamentSettings.WorkDayLength);
                absence.AbsencesSummaryId = absencesSummary.AbsencesSummaryId;

                await _context.Absences.AddAsync(absence);
            }

            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0 ? new AddAbsenceResponse(AbsenceVeryficationResult.Valid) : null;
        }
    }
}

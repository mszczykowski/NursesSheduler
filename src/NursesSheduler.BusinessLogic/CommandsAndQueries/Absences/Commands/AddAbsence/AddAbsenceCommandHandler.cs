using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure.Providers;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.BusinessLogic.Exceptions;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Commands.AddAbsence
{
    internal sealed class AddAbsenceCommandHandler : IRequestHandler<AddAbsenceRequest, AddAbsenceResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<AddAbsenceRequest> _validator;
        private readonly IWorkTimeService _workTimeService;
        private readonly IAbsencesService _absencesService;
        private readonly ICalendarService _calendarService;
        private readonly IDepartamentSettingsProvider _settingsManager;

        public AddAbsenceCommandHandler(IApplicationDbContext context, IMapper mapper, 
            IValidator<AddAbsenceRequest> validator, IWorkTimeService workTimeService, IAbsencesService absencesService,
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

            var absences = _absencesService.GetAbsencesFromAddAbsenceRequest(request.From, request.To, 
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

            var departamentSettings = await _settingsManager.GetDepartamentSettings(absencesSummary.Nurse.DepartamentId);

            foreach (var absence in absences)
            {
                var days = await _calendarService.GetDaysFromDayNumbers(absence.MonthNumber, absencesSummary.Year, 
                    absence.Days);
                absence.WorkTimeToAssign = _workTimeService.GetWorkTimeFromDays(days, departamentSettings.WorkingTime);

                absencesSummary.Absences.Add(absence);
            }

            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0 ? new AddAbsenceResponse(AbsenceVeryficationResult.Valid) : null;
        }
    }
}

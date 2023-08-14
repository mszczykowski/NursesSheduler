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
        private readonly IWorkTimeService _workTimeService;
        private readonly IAbsencesService _absencesService;
        private readonly ICalendarService _calendarService;
        private readonly IDepartamentSettingsProvider _departamentSettingsProvider;

        public AddAbsenceCommandHandler(IApplicationDbContext context, IMapper mapper, 
            IValidator<AddAbsenceRequest> validator, IWorkTimeService workTimeService, IAbsencesService absencesService,
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

        public async Task<AddAbsenceResponse> Handle(AddAbsenceRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
                
            var absencesSummary = await _context.AbsencesSummaries
                .Include(s => s.Absences)
                .Include(s => s.Nurse)
                .FirstOrDefaultAsync(s => s.NurseId == request.NurseId && s.Year == request.From.Year)
                ?? throw new EntityNotFoundException(nameof(AbsencesSummary));

            var absences = _absencesService.GetAbsencesFromDates(request.From, request.To,
                absencesSummary.AbsencesSummaryId, request.Type);

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
                return new AddAbsenceResponse
                {
                    VeryficationResult = veryficationResult,
                };
            }

            var departamentSettings = await _departamentSettingsProvider.GetCachedDataAsync(absencesSummary.Nurse.DepartamentId);

            foreach (var absence in absences)
            {
                var days = await _calendarService.GetDaysFromDayNumbersAsync(absencesSummary.Year, absence.Month, 
                    absence.Days);
                absence.WorkTimeToAssign = _workTimeService.GetWorkTimeFromDays(days, departamentSettings);
                absence.AbsencesSummaryId = absencesSummary.AbsencesSummaryId;

                await _context.Absences.AddAsync(absence);
            }

            var result = await _context.SaveChangesAsync(cancellationToken);

            return new AddAbsenceResponse
            {
                VeryficationResult = veryficationResult,
                Absences = result > 0 ? _mapper.Map<IEnumerable<AddAbsenceResponse.AbsenceResponse>>(absences) : null,
            };
        }
    }
}

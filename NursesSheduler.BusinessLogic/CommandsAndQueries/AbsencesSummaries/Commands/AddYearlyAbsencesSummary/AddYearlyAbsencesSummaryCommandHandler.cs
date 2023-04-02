using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Exceptions;
using NursesScheduler.Domain.DomainModels;
using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Commands.AddYearlyAbsencesSummary
{
    public sealed class AddYearlyAbsencesSummaryCommandHandler : IRequestHandler<AddYearlyAbsencesSummaryRequest,
                                                                                        AddYearlyAbsencesSummaryResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AddYearlyAbsencesSummaryCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AddYearlyAbsencesSummaryResponse> Handle(AddYearlyAbsencesSummaryRequest request,
                                                                                    CancellationToken cancellationToken)
        {
            var nurse = await _context.Nurses.Include(n => n.YearlyAbsencesSummaries)
                .ThenInclude(y => y.Absences)
                .FirstOrDefaultAsync(n => n.NurseId == request.NurseId)
                ?? throw new EntityNotFoundException(request.NurseId, nameof(Nurse));

            if (nurse.YearlyAbsencesSummaries != null && nurse.YearlyAbsencesSummaries.Any(y => y.Year == request.Year))
                throw new EntityAlreadyExistsException(request.Year, nameof(AbsencesSummary));

            var currentYearSummary = new AbsencesSummary
            {
                NurseId = nurse.NurseId,
                Year = request.Year,
                PTODays = nurse.PTOentitlement,
                PTO = nurse.PTOentitlement * TimeSpan.FromDays(1),
                PTOLeftFromPreviousYear = GetPreviousYearPTO(nurse, request.Year),
            };

            nurse.YearlyAbsencesSummaries.Add(currentYearSummary);

            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0 ? _mapper.Map<AddYearlyAbsencesSummaryResponse>(currentYearSummary) : null;
        }

        private TimeSpan GetPreviousYearPTO(Nurse nurse, int currentYear)
        {
            var previousSummary = nurse.YearlyAbsencesSummaries.FirstOrDefault(y => y.Year == currentYear - 1);

            if (previousSummary == null)
                return TimeSpan.Zero;

            var sum = TimeSpan.Zero;
            foreach (var absence in previousSummary.Absences)
            {
                if (absence.Type == AbsenceTypes.PersonalTimeOff)
                    sum += absence.AssignedWorkingHours;
            }

            return previousSummary.PTO - sum + previousSummary.PTOLeftFromPreviousYear;
        }
    }
}

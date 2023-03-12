using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Exceptions;
using NursesScheduler.BusinessLogic.Interfaces.Infrastructure;
using NursesScheduler.Domain.DomainModels;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.YearlyAbsencesSummaries.Commands.AddYearlyAbsencesSummary
{
    public sealed class AddYearlyAbsencesSummaryCommandHandler : IRequestHandler<AddYearlyAbsencesSummaryRequest, AddYearlyAbsencesSummaryResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AddYearlyAbsencesSummaryCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AddYearlyAbsencesSummaryResponse> Handle(AddYearlyAbsencesSummaryRequest request, CancellationToken cancellationToken)
        {
            var nurse = await _context.Nurses.Include(n => n.YearlyAbsencesSummary).FirstOrDefaultAsync(n => n.NurseId == request.NurseId)
                ?? throw new EntityNotFoundException(request.NurseId, nameof(Nurse));

            if (nurse.YearlyAbsencesSummary != null && nurse.YearlyAbsencesSummary.Any(y => y.Year == request.Year))
                throw new EntityAlreadyExistsException(request.Year, nameof(YearlyAbsencesSummary));

            var currentYearSummary = new YearlyAbsencesSummary
            {
                NurseId = nurse.NurseId,
                Year = request.Year,
                PTODays = nurse.PTOentitlement,
                PTO = nurse.PTOentitlement * TimeSpan.FromDays(1),
                PTOUsed = TimeSpan.Zero,
                PTOLeftFromPreviousYear = GetPreviousYearPTO(nurse, request.Year),
            };

            nurse.YearlyAbsencesSummary.Add(currentYearSummary);

            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0 ? _mapper.Map<AddYearlyAbsencesSummaryResponse>(currentYearSummary) : null;
        }

        private TimeSpan GetPreviousYearPTO(Nurse nurse, int currentYear)
        {
            var previousSummary = nurse.YearlyAbsencesSummary.FirstOrDefault(y => y.Year == currentYear - 1);

            if (previousSummary == null)
                return TimeSpan.Zero;

            return previousSummary.PTO - previousSummary.PTOUsed + previousSummary.PTOLeftFromPreviousYear;
        }
    }
}

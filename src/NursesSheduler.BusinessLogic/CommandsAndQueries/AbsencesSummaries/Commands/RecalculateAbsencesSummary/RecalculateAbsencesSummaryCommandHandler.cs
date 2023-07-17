using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Exceptions;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Commands.RecalculateAbsencesSummary
{
    internal sealed class RecalculateAbsencesSummaryCommandHandler : IRequestHandler<RecalculateAbsencesSummaryRequest, 
                                                                                    RecalculateAbsencesSummaryResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public RecalculateAbsencesSummaryCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<RecalculateAbsencesSummaryResponse> Handle(RecalculateAbsencesSummaryRequest request,
                                                                                    CancellationToken cancellationToken)
        {
            var currentSummary = await _context.AbsencesSummaries
                                            .Include(s => s.Absences)
                                            .Include(s => s.Nurse)
                                            .FirstOrDefaultAsync(s => s.AbsencesSummaryId == request.AbsencesSummaryId);

            if (currentSummary == null)
                throw new EntityNotFoundException(request.AbsencesSummaryId, nameof(AbsencesSummary));

            var previousSummary = await _context.AbsencesSummaries
                                            .FirstOrDefaultAsync(s => s.NurseId == currentSummary.NurseId
                                                                    && s.Year == currentSummary.Year - 1);

            var response = new RecalculateAbsencesSummaryResponse();

            if(previousSummary == null)
            {
                response.PTOTimeLeftFromPreviousYear = TimeSpan.Zero;
            }
            else
            {
                response.PTOTimeLeftFromPreviousYear = previousSummary.PTOTimeLeftFromPreviousYear
                                                                + previousSummary.PTOTime - previousSummary.PTOTimeUsed;
            }
            response.PTODays = currentSummary.Nurse.PTOentitlement;
            foreach(var absence in currentSummary.Absences)
            {
                response.PTOTimeUsed += absence.AssignedWorkingHours;
            }

            return response;
        }
    }
}

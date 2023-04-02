using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Exceptions;
using NursesScheduler.Domain.DomainModels;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Commands.DeleteAbsence
{
    internal sealed class DeleteAbsenceCommandHandler : IRequestHandler<DeleteAbsenceRequest, DeleteAbsenceResponse>
    {
        private readonly IApplicationDbContext _context;

        public DeleteAbsenceCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DeleteAbsenceResponse> Handle(DeleteAbsenceRequest request, CancellationToken cancellationToken)
        {
            var absence = await _context.Absences.FirstOrDefaultAsync(a => a.AbsenceId == request.AbsenceId)
                ?? throw new EntityNotFoundException(request.AbsenceId, nameof(Absence));

            _context.Absences.Remove(absence);

            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0 ? new DeleteAbsenceResponse(true) : new DeleteAbsenceResponse(false);
        }
    }
}

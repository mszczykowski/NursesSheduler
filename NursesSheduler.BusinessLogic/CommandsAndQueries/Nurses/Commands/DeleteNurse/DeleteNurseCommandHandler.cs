using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Exceptions;
using NursesScheduler.BusinessLogic.Interfaces.Infrastructure;
using NursesScheduler.Domain.DomainModels;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Nurses.Commands.DeleteNurse
{
    public class DeleteNurseCommandHandler : IRequestHandler<DeleteNurseRequest, DeleteNurseResponse>
    {
        private readonly IApplicationDbContext _context;

        public DeleteNurseCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DeleteNurseResponse> Handle(DeleteNurseRequest request, CancellationToken cancellationToken)
        {
            var nurse = await _context.Nurses.Include(n => n.Shifts).FirstOrDefaultAsync(n => n.NurseId == request.NurseId)
                ?? throw new EntityNotFoundException(request.NurseId, nameof(Nurse));


            //soft delete if nurse is assinged to any shift
            if (nurse.Shifts.Any())
            {
                nurse.IsDeleted = true;
            }
            else
            {
                _context.Nurses.Remove(nurse);
            }

            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0 ? new DeleteNurseResponse(true) : new DeleteNurseResponse(false);
        }
    }
}

using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Exceptions;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Commands.DeleteDepartament
{
    internal class DeleteDepartamentCommandHandler : IRequestHandler<DeleteDepartamentRequest, DeleteDepartamentResponse>
    {
        private readonly IApplicationDbContext _context;

        public DeleteDepartamentCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DeleteDepartamentResponse> Handle(DeleteDepartamentRequest request, CancellationToken cancellationToken)
        {
            var departament = await _context.Departaments.FirstOrDefaultAsync(d => d.DepartamentId == request.DepartamentId)
                ?? throw new EntityNotFoundException(request.DepartamentId, nameof(Departament));

            _context.Departaments.Remove(departament);

            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0 ? new DeleteDepartamentResponse(true) : new DeleteDepartamentResponse(false);
        }
    }
}

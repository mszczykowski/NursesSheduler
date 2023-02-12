using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Interfaces.Infrastructure;
using NursesScheduler.Domain.DatabaseModels;
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
            var departament = await _context.Departaments.FirstOrDefaultAsync(d => d.Id == request.Id)
                ?? throw new EntityNotFoundException(request.Id, nameof(Departament));

            _context.Departaments.Remove(departament);

            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0 ? new DeleteDepartamentResponse(true) : new DeleteDepartamentResponse(false);
        }
    }
}

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NursesScheduler.BusinessLogic.Nurses.Queries.GetAllNurses
{
    public class GetAllNursesQueryHandler : IRequestHandler<GetAllNursesRequest, List<GetAllNursesResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllNursesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<GetAllNursesResponse>> Handle(GetAllNursesRequest request, CancellationToken cancellationToken)
        {
            var nurses = await _context.Nurses
                .Include(n => n.Departament)
                .ToListAsync();

            return _mapper.Map<List<GetAllNursesResponse>>(nurses);
        }
    }
}

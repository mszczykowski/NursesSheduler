using AutoMapper;
using MediatR;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Commands.UpsertSchedule
{
    internal class UpsertScheduleCommandHandler : IRequestHandler<UpsertScheduleRequest, UpsertScheduleResponse>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _applicationDbContext;

        public Task<UpsertScheduleResponse> Handle(UpsertScheduleRequest request, CancellationToken cancellationToken)
        {
            var schedule = _mapper.Map<Schedule>(request);
        }
    }
}

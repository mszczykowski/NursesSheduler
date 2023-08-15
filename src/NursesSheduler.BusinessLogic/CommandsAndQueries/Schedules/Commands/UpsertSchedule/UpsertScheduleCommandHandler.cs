using AutoMapper;
using MediatR;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Commands.UpsertSchedule
{
    internal sealed class UpsertScheduleCommandHandler : IRequestHandler<UpsertScheduleRequest, UpsertScheduleResponse>
    {
        private readonly IMapper _mapper;
        private readonly ISchedulesService _schedulesService;

        public UpsertScheduleCommandHandler(IMapper mapper, ISchedulesService schedulesService)
        {
            _mapper = mapper;
            _schedulesService = schedulesService;
        }

        public async Task<UpsertScheduleResponse?> Handle(UpsertScheduleRequest request, 
            CancellationToken cancellationToken)
        {
            var updatedSchedule = _mapper.Map<Schedule>(request);

            return await _schedulesService.UpsertSchedule(updatedSchedule, cancellationToken) > 0 
                ? _mapper.Map<UpsertScheduleResponse>(updatedSchedule) : null;
        }
    }
}

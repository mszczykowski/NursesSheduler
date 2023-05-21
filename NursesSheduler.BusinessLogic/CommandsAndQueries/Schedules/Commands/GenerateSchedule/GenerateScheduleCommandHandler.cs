﻿using AutoMapper;
using MediatR;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Commands.GenerateSchedule
{
    internal sealed class GenerateScheduleCommandHandler : IRequestHandler<GenerateScheduleRequest,
        GenerateScheduleResponse>
    {
        private readonly IMapper _mapper;

        public Task<GenerateScheduleResponse> Handle(GenerateScheduleRequest request, CancellationToken cancellationToken)
        {
            var schedule = _mapper.Map<Schedule>(request);



            return null;
        }
    }
}

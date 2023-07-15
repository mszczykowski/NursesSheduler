using AutoMapper;
using MediatR;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.RecalculateScheduleHours
{
    internal sealed class RecalculateScheduleHoursCommandHandler : IRequestHandler<RecalculateScheduleHoursRequest,
        RecalculateScheduleHoursResponse>
    {
        private readonly IMapper _mapper;
        public async Task<RecalculateScheduleHoursResponse> Handle(RecalculateScheduleHoursRequest request,
            CancellationToken cancellationToken)
        {
            var scheduleNurse = _mapper.Map<ScheduleNurse>(request.ScheduleNurse);
            var morningShifts = _mapper.Map<MorningShift>(request.MorningShifts);
            var days = _mapper.Map<Day>(request.Days);

            throw new NotImplementedException();
        }
    }
}

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure.Providers;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Exceptions;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Commands.UpsertSchedule
{
    internal class UpsertScheduleCommandHandler : IRequestHandler<UpsertScheduleRequest, UpsertScheduleResponse>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IScheduleStatsProvider _scheduleStatsProvider;

        public async Task<UpsertScheduleResponse?> Handle(UpsertScheduleRequest request, 
            CancellationToken cancellationToken)
        {
            var schedule = _mapper.Map<Schedule>(request);

            var quarter = await _applicationDbContext.Quarters.FindAsync(request.QuarterId)
                ?? throw new EntityNotFoundException(request.QuarterId, nameof(Quarter));

            _scheduleStatsProvider.InvalidateCache(new ScheduleStatsKey
            {
                DepartamentId = quarter.DepartamentId,
                Month = schedule.Month,
                Year = quarter.Year,
            });

            var oldSchedule = await _applicationDbContext.Schedules
                .Include(s => s.ScheduleNurses)
                .ThenInclude(n => n.NurseWorkDays)
                .FirstOrDefaultAsync(s => s.ScheduleId == request.ScheduleId);

            if(oldSchedule is null)
            {
                _applicationDbContext.Schedules.Add(schedule);

                return await _applicationDbContext.SaveChangesAsync(cancellationToken) > 0 
                    ? _mapper.Map<UpsertScheduleResponse>(schedule) : null;
            }

            foreach (var updatedWorkDay in schedule.ScheduleNurses.SelectMany(n => n.NurseWorkDays))
            {
                var oldWorkDay = oldSchedule.ScheduleNurses
                    .SelectMany(n => n.NurseWorkDays)
                    .First(wd => wd.NurseWorkDayId == updatedWorkDay.NurseWorkDayId);

                oldWorkDay.ShiftType = updatedWorkDay.ShiftType;
                
                if(updatedWorkDay.ShiftType == Domain.Enums.ShiftTypes.Morning)
                {
                    oldWorkDay.MorningShiftId = updatedWorkDay.MorningShiftId;
                }
            }

            return await _applicationDbContext.SaveChangesAsync(cancellationToken) > 0 
                ? _mapper.Map<UpsertScheduleResponse>(oldSchedule) : null;
        }
    }
}

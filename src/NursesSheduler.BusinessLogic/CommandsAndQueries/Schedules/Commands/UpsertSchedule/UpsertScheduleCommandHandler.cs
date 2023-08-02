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

        public UpsertScheduleCommandHandler(IMapper mapper, IApplicationDbContext applicationDbContext, 
            IScheduleStatsProvider scheduleStatsProvider)
        {
            _mapper = mapper;
            _applicationDbContext = applicationDbContext;
            _scheduleStatsProvider = scheduleStatsProvider;
        }

        public async Task<UpsertScheduleResponse?> Handle(UpsertScheduleRequest request, 
            CancellationToken cancellationToken)
        {
            var udpatedSchedule = _mapper.Map<Schedule>(request);

            var quarter = await _applicationDbContext.Quarters.FindAsync(request.QuarterId)
                ?? throw new EntityNotFoundException(request.QuarterId, nameof(Quarter));

            _scheduleStatsProvider.InvalidateCache(new ScheduleStatsKey
            {
                DepartamentId = quarter.DepartamentId,
                Month = udpatedSchedule.Month,
                Year = quarter.Year,
            });

            var oldSchedule = await _applicationDbContext.Schedules
                .Include(s => s.ScheduleNurses)
                .ThenInclude(n => n.NurseWorkDays)
                .FirstOrDefaultAsync(s => s.QuarterId == request.QuarterId &&
                    s.Month == request.Month);

            var x = await _applicationDbContext.Schedules.ToListAsync();

            if(oldSchedule is null)
            {
                _applicationDbContext.Schedules.Add(udpatedSchedule);

                return await _applicationDbContext.SaveChangesAsync(cancellationToken) > 0 
                    ? _mapper.Map<UpsertScheduleResponse>(udpatedSchedule) : null;
            }

            foreach (var oldScheduleNurse in oldSchedule.ScheduleNurses)
            {
                var updatedWorkDays = udpatedSchedule
                    .ScheduleNurses
                    .First(n => n.NurseId == oldScheduleNurse.NurseId)
                    .NurseWorkDays;

                foreach(var oldWorkDay in oldScheduleNurse.NurseWorkDays)
                {
                    var updatedWorkDay = updatedWorkDays.First(wd => wd.Day == oldWorkDay.Day);

                    oldWorkDay.ShiftType = updatedWorkDay.ShiftType;

                    if (updatedWorkDay.ShiftType == Domain.Enums.ShiftTypes.Morning)
                    {
                        oldWorkDay.MorningShiftId = updatedWorkDay.MorningShiftId;
                        continue;
                    }

                    oldWorkDay.MorningShift = null;
                    oldWorkDay.MorningShiftId = null;
                }
            }

            return await _applicationDbContext.SaveChangesAsync(cancellationToken) > 0 
                ? _mapper.Map<UpsertScheduleResponse>(oldSchedule) : null;
        }
    }
}

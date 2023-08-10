using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Exceptions;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Commands.CloseSchedule
{
    internal sealed class CloseScheduleCommandHandler : IRequestHandler<CloseScheduleRequest, CloseScheduleResponse>
    {
        private readonly IMapper _mapper;
        private readonly ISchedulesService _schedulesService;
        private readonly IAbsencesService _absencesService;
        private readonly IApplicationDbContext _applicationDbContext;

        public CloseScheduleCommandHandler(IMapper mapper, ISchedulesService schedulesService, IAbsencesService absencesService, IApplicationDbContext applicationDbContext)
        {
            _mapper = mapper;
            _schedulesService = schedulesService;
            _absencesService = absencesService;
            _applicationDbContext = applicationDbContext;
        }

        public async Task<CloseScheduleResponse> Handle(CloseScheduleRequest request, CancellationToken cancellationToken)
        {
            var closedSchedule = _mapper.Map<Schedule>(request);
            closedSchedule.IsClosed = true;

            var quarter = await _applicationDbContext.Quarters
                .Include(q => q.MorningShifts)
                .FirstOrDefaultAsync(q => q.QuarterId == closedSchedule.QuarterId)
                ?? throw new EntityNotFoundException(closedSchedule.QuarterId, nameof(Quarter));

            if (await _schedulesService.UpsertSchedule(closedSchedule, cancellationToken) == 0)
            {
                throw new InvalidOperationException();
            }

            var usedMorningShiftsIds = closedSchedule
                .ScheduleNurses
                .SelectMany(n => n.NurseWorkDays)
                .Where(wd => wd.ShiftType == Domain.Enums.ShiftTypes.Morning)
                .Select(wd => wd.MorningShiftId)
                .Distinct();

            foreach (var morningShiftId in usedMorningShiftsIds)
            {
                quarter.MorningShifts.First(m => m.MorningShiftId == morningShiftId).ReadOnly = true;
            }

            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            _schedulesService.ResolveMorningShifts(closedSchedule, quarter.MorningShifts);

            await _absencesService.AssignTimeOffsWorkTime(closedSchedule, quarter.Year, cancellationToken);

            return _mapper.Map<CloseScheduleResponse>(closedSchedule);
        }
    }
}

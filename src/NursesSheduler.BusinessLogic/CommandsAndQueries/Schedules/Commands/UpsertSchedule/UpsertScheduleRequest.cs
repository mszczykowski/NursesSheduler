using MediatR;
using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Commands.UpsertSchedule
{
    public sealed class UpsertScheduleRequest : IRequest<UpsertScheduleResponse?>
    {
        public int ScheduleId { get; set; }
        public int Month { get; set; }
        public bool IsClosed { get; set; }
        public int QuarterId { get; set; }
        public IEnumerable<ScheduleNurseRequest> ScheduleNurses { get; set; }

        public sealed class ScheduleNurseRequest
        {
            public int ScheduleNurseId { get; set; }
            public int NurseId { get; init; }
            public IEnumerable<NurseWorkDayRequest> NurseWorkDays { get; set; }
        }

        public sealed class NurseWorkDayRequest
        {
            public int NurseWorkDayId { get; set; }
            public int Day { get; set; }
            public bool IsTimeOff { get; set; }
            public ShiftTypes ShiftType { get; set; }
            public int? MorningShiftId { get; set; }
        }
    }
}

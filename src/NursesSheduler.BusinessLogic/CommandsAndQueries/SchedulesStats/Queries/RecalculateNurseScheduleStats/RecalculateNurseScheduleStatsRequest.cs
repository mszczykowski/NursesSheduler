using MediatR;
using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.RecalculateNursesScheduleStats
{
    public sealed class RecalculateNurseScheduleStatsRequest : IRequest<RecalculateNursesScheduleStatsResponse>
    {
        public int DepartamentId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public ScheduleNurseRequest ScheduleNurse { get; set; }

        public sealed class ScheduleNurseRequest
        {
            public int ScheduleNurseId { get; set; }
            public int NurseId { get; init; }
            public int ScheduleId { get; init; }
            public IEnumerable<NurseWorkDayRequest> NurseWorkDays { get; set; }
        }

        public sealed class NurseWorkDayRequest
        {
            public int NurseWorkDayId { get; set; }
            public int ScheduleNurseId { get; set; }
            public int Day { get; set; }
            public bool IsTimeOff { get; set; }
            public ShiftTypes ShiftType { get; set; }
            public int MorningShiftId { get; set; }
        }
    }
}

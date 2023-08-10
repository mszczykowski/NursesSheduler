using MediatR;
using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.ScheduleValidations.Queries.ValidateScheduleNurse
{
    public sealed class ValidateScheduleNurseRequest : IRequest<IEnumerable<ValidateScheduleNurseResponse>>
    {
        public int DepartamentId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public ScheduleNurseRequest ScheduleNurse { get; set; }

        public sealed class ScheduleNurseRequest
        {
            public int NurseId { get; init; }
            public int ScheduleId { get; init; }
            public IEnumerable<NurseWorkDayRequest> NurseWorkDays { get; set; }
        }

        public sealed class NurseWorkDayRequest
        {
            public int Day { get; set; }
            public bool IsTimeOff { get; set; }
            public ShiftTypes ShiftType { get; set; }
            public MorningShiftRequest MorningShift { get; set; }
        }

        public sealed class MorningShiftRequest
        {
            public MorningShiftIndex Index { get; set; }
            public TimeSpan ShiftLength { get; set; }
        }
    }
}

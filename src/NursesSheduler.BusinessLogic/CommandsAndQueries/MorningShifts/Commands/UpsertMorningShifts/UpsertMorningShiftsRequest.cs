using MediatR;
using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.MorningShifts.Commands.UpsertMorningShifts
{
    public sealed class UpsertMorningShiftsRequest : IRequest<IEnumerable<UpsertMorningShiftsResponse>>
    {
        public int QuarterId { get; set; }
        public IEnumerable<MorningShiftRequest> MorningShifts { get; init; }

        public sealed class MorningShiftRequest
        {
            public MorningShiftIndex Index { get; set; }
            public TimeSpan ShiftLength { get; set; }
        }
    }
}

using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.MorningShifts.Commands.UpsertMorningShifts
{
    public sealed class UpsertMorningShiftsResponse
    {
        public int MorningShiftId { get; set; }
        public MorningShiftIndex Index { get; set; }
        public TimeSpan ShiftLength { get; set; }
    }
}

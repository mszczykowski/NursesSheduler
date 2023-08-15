using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.MorningShifts.Queries.GetMorningShifts
{
    public sealed class GetMorningShiftsResponse
    {
        public int MorningShiftId { get; set; }
        public MorningShiftIndex Index { get; set; }
        public TimeSpan ShiftLength { get; set; }
    }
}

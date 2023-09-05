using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.MorningShifts.Commands.CalculateMorningShifts
{
    public sealed class CalculateMorningShiftsResponse
    {
        public MorningShiftIndex Index { get; set; }
        public TimeSpan ShiftLength { get; set; }
    }
}

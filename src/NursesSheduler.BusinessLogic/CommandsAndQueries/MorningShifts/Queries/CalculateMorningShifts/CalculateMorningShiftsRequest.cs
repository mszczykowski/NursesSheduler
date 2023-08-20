using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.MorningShifts.Queries.CalculateMorningShifts
{
    public sealed class CalculateMorningShiftsRequest : IRequest<IEnumerable<CalculateMorningShiftsResponse>>
    {
        public int DepartamentId { get; set; }
        public TimeSpan TimeForMorningShifts { get; set; }
    }
}

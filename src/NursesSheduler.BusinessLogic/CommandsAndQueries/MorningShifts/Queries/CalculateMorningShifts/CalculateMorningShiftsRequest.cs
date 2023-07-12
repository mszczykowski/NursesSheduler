using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.MorningShifts.Queries.CalculateMorningShifts
{
    public sealed class CalculateMorningShiftsRequest : IRequest<ICollection<CalculateMorningShiftsResponse>>
    {
        public TimeSpan TimeForMorningShifts { get; set; }
        public int DepartamentId { get; set; }
    }
}

using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.MorningShifts.Queries.GetMorningShifts
{
    public sealed class GetMorningShiftsRequest : IRequest<IEnumerable<GetMorningShiftsResponse>>
    {
        public int QuarterId { get; set; }
    }
}

using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Quarters.Commands.UpsertQuarter
{
    public sealed class UpsertQuarterRequest : IRequest<UpsertQuarterResponse>
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int DepartamentId { get; set; }
    }
}

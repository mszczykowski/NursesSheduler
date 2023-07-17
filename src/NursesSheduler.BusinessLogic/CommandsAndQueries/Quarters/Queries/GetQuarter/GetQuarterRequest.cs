using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Quarters.Queries.GetQuarter
{
    public sealed class GetQuarterRequest : IRequest<GetQuarterResponse?>
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int DepartamentId { get; set; }
    }
}

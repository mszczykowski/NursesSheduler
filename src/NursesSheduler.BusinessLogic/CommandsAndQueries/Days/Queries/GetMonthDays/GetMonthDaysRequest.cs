using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Days.Queries.GetMonthDays
{
    public sealed class GetMonthDaysRequest : IRequest<IEnumerable<GetMonthDaysResponse>>
    {
        public int Year { get; set; }
        public int Month { get; set; }
    }
}

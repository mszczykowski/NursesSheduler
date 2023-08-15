using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Quarters.Commands.AddQuarter
{
    public sealed class AddQuarterRequest : IRequest<AddQuarterResponse?>
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int DepartamentId { get; set; }
    }
}

using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule
{
    public class GetScheduleRequest : IRequest<GetScheduleResponse>
    {
        public int DepartamentId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }
}

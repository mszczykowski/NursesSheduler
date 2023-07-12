using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule
{
    public class GetScheduleRequest : IRequest<GetScheduleResponse>
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int DepartamentId { get; set; }
    }
}

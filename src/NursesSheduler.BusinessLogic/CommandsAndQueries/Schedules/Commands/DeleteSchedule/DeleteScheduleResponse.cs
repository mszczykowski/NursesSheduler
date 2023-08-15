namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Commands.DeleteSchedule
{
    public sealed class DeleteScheduleResponse
    {
        public bool Success { get; init; }

        public DeleteScheduleResponse(bool success)
        {
            Success = success;
        }
    }
}

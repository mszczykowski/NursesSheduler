namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Nurses.Commands.DeleteNurse
{
    public sealed class DeleteNurseResponse
    {
        public bool Success { get; set; }

        public DeleteNurseResponse(bool success)
        {
            Success = success;
        }
    }
}

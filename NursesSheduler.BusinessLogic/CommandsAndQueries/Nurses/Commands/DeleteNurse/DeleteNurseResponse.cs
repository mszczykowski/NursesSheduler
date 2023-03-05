namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Nurses.Commands.DeleteNurse
{
    public class DeleteNurseResponse
    {
        public bool Success { get; set; }

        public DeleteNurseResponse(bool success)
        {
            Success = success;
        }
    }
}

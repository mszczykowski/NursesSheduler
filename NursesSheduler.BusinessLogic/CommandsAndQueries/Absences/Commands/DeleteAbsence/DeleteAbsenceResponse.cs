namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Commands.DeleteAbsence
{
    public sealed class DeleteAbsenceResponse
    {
        public bool Success { get; set; }

        public DeleteAbsenceResponse(bool success)
        {
            Success = success;
        }
    }
}

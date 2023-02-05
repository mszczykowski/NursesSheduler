namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Commands.DeleteDepartament
{
    internal class DeleteDepartamentResponse
    {
        public bool IsDeleted { get; set; }

        public DeleteDepartamentResponse(bool isDeleted)
        {
            IsDeleted = isDeleted;
        }
    }
}

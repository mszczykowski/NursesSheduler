namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Commands.EditDepartament
{
    public sealed class EditDepartamentResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FirstQuarterStart { get; set; }
    }
}

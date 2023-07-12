namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Commands.PickDepartament
{
    public sealed class PickDepartamentResponse
    {
        public int DepartamentId { get; set; }
        public string Name { get; set; }
        public int CreationYear { get; set; }
        public int FirstQuarterStart { get; set; }
    }
}

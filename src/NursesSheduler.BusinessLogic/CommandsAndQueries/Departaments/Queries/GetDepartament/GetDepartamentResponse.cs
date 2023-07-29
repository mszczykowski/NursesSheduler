namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Queries.GetDepartament
{
    public sealed class GetDepartamentResponse
    {
        public int DepartamentId { get; set; }
        public string Name { get; set; }
        public int FirstQuarterStart { get; set; }
        public int DefaultGeneratorRetryValue { get; set; }
    }
}

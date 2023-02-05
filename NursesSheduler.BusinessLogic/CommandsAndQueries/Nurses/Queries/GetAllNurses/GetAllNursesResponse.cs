namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Nurses.Queries.GetAllNurses
{
    public class GetAllNursesResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DepartamentResponse Departament { get; set; }
        public class DepartamentResponse
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}

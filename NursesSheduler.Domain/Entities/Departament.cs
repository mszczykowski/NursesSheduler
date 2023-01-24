namespace NursesScheduler.Domain.Entities
{
    public sealed class Departament
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
    }
}

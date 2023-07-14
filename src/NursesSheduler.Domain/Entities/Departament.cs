namespace NursesScheduler.Domain.Entities
{
    public record Departament
    {
        public int DepartamentId { get; set; }
        public string Name { get; set; }
        public int CreationYear { get; set; }
        public int FirstQuarterStart { get; set; }

        public virtual ICollection<Nurse> Nurses { get; set; }
        public virtual ICollection<Quarter> Quarters { get; set; }
        public virtual DepartamentSettings DepartamentSettings { get; set; }
    }
}

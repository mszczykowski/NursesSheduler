using NursesScheduler.Domain.Abstractions;

namespace NursesScheduler.Domain.Entities
{
    public record Departament : IEntity
    {
        public string Name { get; set; }
        public int CreationYear { get; set; }

        public virtual ICollection<Nurse> Nurses { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }

        public virtual DepartamentSettings DepartamentSettings { get; set; }
    }
}

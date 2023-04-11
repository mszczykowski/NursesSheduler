using NursesScheduler.Domain.DomainModels.Schedules;

namespace NursesScheduler.Domain.DomainModels
{
    public class Departament
    {
        public int DepartamentId { get; set; }
        public string Name { get; set; }
        public int CreationYear { get; set; }

        public virtual ICollection<Nurse> Nurses { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }

        public DepartamentSettings DepartamentSettings { get; set; }
    }
}

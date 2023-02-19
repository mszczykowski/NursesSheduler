using NursesScheduler.Domain.DatabaseModels.Schedules;

namespace NursesScheduler.Domain.DatabaseModels
{
    public class Departament
    {
        public int DepartamentId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Nurse> Nurses { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}

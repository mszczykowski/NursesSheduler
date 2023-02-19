using NursesScheduler.Domain.DatabaseModels.Schedules;
using NursesScheduler.Domain.Interfaces;

namespace NursesScheduler.Domain.DatabaseModels
{
    public class Nurse : ISoftDelete
    {
        public int NurseId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsActive { get; set; }
        public int? DepartamentId { get; set; }
        public Departament Departament { get; set; }

        public virtual ICollection<Shift> Shifts { get; set; }
        public virtual ICollection<TimeOff> TimeOffs { get; set; }

        public bool IsDeleted { get; set; }
    }
}

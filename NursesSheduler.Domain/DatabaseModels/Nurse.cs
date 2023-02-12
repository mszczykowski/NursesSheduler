using NursesScheduler.Domain.DatabaseModels.Schedules;
using NursesScheduler.Domain.Interfaces;

namespace NursesScheduler.Domain.DatabaseModels
{
    public sealed class Nurse : ISoftDelete
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public int? DepartamentId { get; set; }
        public Departament Departament { get; set; }

        public ICollection<Shift> Shifts { get; set; }
        public ICollection<TimeOff> TimeOffs { get; set; }

        public bool IsDeleted { get; set; }
    }
}

using NursesScheduler.Domain.DomainModels.Schedules;
using NursesScheduler.Domain.Interfaces;

namespace NursesScheduler.Domain.DomainModels
{
    public class Nurse : ISoftDelete
    {
        public int NurseId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int DepartamentId { get; set; }
        public int PTOentitlement { get; set; }
        public Departament Departament { get; set; }

        public virtual ICollection<Shift> Shifts { get; set; }
        public virtual ICollection<AbsencesSummary> YearlyAbsencesSummaries { get; set; }

        public bool IsDeleted { get; set; }
    }
}

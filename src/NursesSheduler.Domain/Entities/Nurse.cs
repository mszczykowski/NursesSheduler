using NursesScheduler.Domain.Interfaces;

namespace NursesScheduler.Domain.Entities
{
    public class Nurse : ISoftDelete
    {
        public int NurseId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int PTOentitlement { get; set; }

        public int DepartamentId { get; set; }
        public virtual Departament Departament { get; set; }

        public virtual ICollection<NurseWorkDay> Shifts { get; set; }
        public virtual ICollection<AbsencesSummary> AbsencesSummaries { get; set; }

        public bool IsDeleted { get; set; }
    }
}

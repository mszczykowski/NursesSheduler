using NursesScheduler.Domain.Abstractions;
using NursesScheduler.Domain.Enums;

namespace NursesScheduler.Domain.Entities
{
    public record Nurse : ISoftDelete
    {
        public int NurseId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int PTOentitlement { get; set; }
        public Teams Team { get; set; }

        public int NightShiftsBalance { get; set; }
        public TimeSpan HolidayHoursBalance { get; set; }

        public bool IsDeleted { get; set; }

        public int DepartamentId { get; set; }
        public virtual Departament Departament { get; set; }

        public virtual ICollection<NurseWorkDay> NurseWorkDays { get; set; }
        public virtual ICollection<AbsencesSummary> AbsencesSummaries { get; set; }
    }
}

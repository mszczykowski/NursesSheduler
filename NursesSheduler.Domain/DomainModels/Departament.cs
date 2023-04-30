﻿namespace NursesScheduler.Domain.DomainModels
{
    public class Departament
    {
        public int DepartamentId { get; set; }
        public string Name { get; set; }
        public int CreationYear { get; set; }

        public virtual ICollection<Nurse> Nurses { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }

        public virtual DepartamentSettings DepartamentSettings { get; set; }
    }
}

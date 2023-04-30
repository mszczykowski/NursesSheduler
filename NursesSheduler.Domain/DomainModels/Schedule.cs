﻿namespace NursesScheduler.Domain.DomainModels
{
    public class Schedule
    {
        public int ScheduleId { get; set; }
        public int MonthNumber { get; set; }
        public int MonthInQuarter { get; set; }
        public int Year { get; set; }
        public int QuarterId{ get; set; }
        public virtual Quarter Quarter { get; set; }
        public TimeSpan WorkTimeInMonth { get; set; }
        public TimeSpan TimeOffAvailableToAssgin { get; set; }
        public TimeSpan TimeOffAssigned { get; set; }
        public int SettingsVersion { get; set; }

        public ICollection<int> Holidays { get; set; }
        public virtual ICollection<ScheduleNurse> ScheduleNurses{ get; set; }


        public int DepartamentId { get; set; }
        public virtual Departament Departament { get; set; }
    }
}

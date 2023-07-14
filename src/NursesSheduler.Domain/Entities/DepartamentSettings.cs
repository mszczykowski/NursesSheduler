﻿namespace NursesScheduler.Domain.Entities
{
    public record DepartamentSettings
    {
        public int DepartamentSettingsId { get; set; }
        public TimeSpan WorkingTime { get; set; }
        public TimeSpan MaximalWeekWorkingTime { get; set; }
        public TimeSpan MinmalShiftBreak { get; set; }
        public int TargetNumberOfNursesOnShift { get; set; }
        public TimeSpan TargetMinimalMorningShiftLenght { get; set; }
        public int DefaultGeneratorRetryValue { get; set; }
        public int SettingsVersion { get; set; }
        public TimeSpan DayShiftHolidayEligibleHours { get; set; }
        public TimeSpan NightShiftHolidayEligibleHours { get; set; }
        public int FirstQuarterStart { get; set; }

        public int DepartamentId { get; set; }
        public virtual Departament Departament { get; set; }

        public DepartamentSettings()
        {

        }
        public DepartamentSettings(int firstQuarterStart)
        {
            WorkingTime = new TimeSpan(7, 35, 0);
            MaximalWeekWorkingTime = TimeSpan.FromHours(24);
            MinmalShiftBreak = TimeSpan.FromHours(12);
            TargetNumberOfNursesOnShift = 4;
            TargetMinimalMorningShiftLenght = TimeSpan.FromHours(6);
            DefaultGeneratorRetryValue = 4;
            SettingsVersion = 1;
            DayShiftHolidayEligibleHours = TimeSpan.FromHours(12);
            NightShiftHolidayEligibleHours = TimeSpan.FromHours(4);
            FirstQuarterStart = firstQuarterStart;
        }

        public virtual bool Equals(DepartamentSettings? other)
        {
            if (other == null)
                return false;

            return WorkingTime == other.WorkingTime &&
                MaximalWeekWorkingTime == other.MaximalWeekWorkingTime &&
                MinmalShiftBreak == other.MinmalShiftBreak &&
                FirstQuarterStart == other.FirstQuarterStart &&
                DayShiftHolidayEligibleHours == other.DayShiftHolidayEligibleHours &&
                NightShiftHolidayEligibleHours == other.NightShiftHolidayEligibleHours &&
                TargetNumberOfNursesOnShift == other.TargetNumberOfNursesOnShift &&
                TargetMinimalMorningShiftLenght == other.TargetMinimalMorningShiftLenght &&
                DefaultGeneratorRetryValue == other.DefaultGeneratorRetryValue;

        }
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace NursesScheduler.Domain.Entities
{
    public record DepartamentSettings
    {
        public int DepartamentSettingsId { get; set; }

        public TimeSpan WorkDayLength { get; set; }
        public TimeSpan MaximalWeekWorkDayLength { get; set; }
        public TimeSpan MinmalShiftBreak { get; set; }
        public int TargetMinNumberOfNursesOnShift { get; set; }
        public TimeSpan TargetMinimalMorningShiftLenght { get; set; }
        public int DefaultGeneratorRetryValue { get; set; }
        public TimeSpan DayShiftHolidayEligibleHours { get; set; }
        public TimeSpan NightShiftHolidayEligibleHours { get; set; }

        public int DepartamentId { get; set; }
        public virtual Departament Departament { get; set; }

        [NotMapped]
        public int FirstQuarterStart => Departament.FirstQuarterStart;

        public DepartamentSettings()
        {
            WorkDayLength = new TimeSpan(7, 35, 0);
            MaximalWeekWorkDayLength = TimeSpan.FromHours(24);
            MinmalShiftBreak = TimeSpan.FromHours(12);
            TargetMinNumberOfNursesOnShift = 4;
            TargetMinimalMorningShiftLenght = TimeSpan.FromHours(6);
            DefaultGeneratorRetryValue = 4;
            DayShiftHolidayEligibleHours = TimeSpan.FromHours(12);
            NightShiftHolidayEligibleHours = TimeSpan.FromHours(4);
        }
    }
}

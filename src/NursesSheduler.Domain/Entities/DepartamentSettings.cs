using System.ComponentModel.DataAnnotations.Schema;

namespace NursesScheduler.Domain.Entities
{
    public record DepartamentSettings
    {
        public int DepartamentSettingsId { get; set; }

        public TimeSpan WorkDayLength { get; set; }
        public TimeSpan MaximumWeekWorkTimeLength { get; set; }
        public TimeSpan MinimalShiftBreak { get; set; }
        public TimeSpan TargetMinimalMorningShiftLenght { get; set; }
        public TimeSpan DayShiftHolidayEligibleHours { get; set; }
        public TimeSpan NightShiftHolidayEligibleHours { get; set; }
        public int TargetMinNumberOfNursesOnShift { get; set; }
        public int DefaultGeneratorRetryValue { get; set; }
        public bool UseTeams { get; set; }
        public int FirstQuarterStart { get; set; }

        public int DepartamentId { get; set; }
        public virtual Departament Departament { get; set; }

        public DepartamentSettings()
        {

        }

        public DepartamentSettings(int firstQuarerStart)
        {
            WorkDayLength = new TimeSpan(7, 35, 0);
            MaximumWeekWorkTimeLength = TimeSpan.FromHours(48);
            MinimalShiftBreak = TimeSpan.FromHours(12);
            TargetMinNumberOfNursesOnShift = 4;
            TargetMinimalMorningShiftLenght = TimeSpan.FromHours(6);
            DefaultGeneratorRetryValue = 4;
            DayShiftHolidayEligibleHours = TimeSpan.FromHours(12);
            NightShiftHolidayEligibleHours = TimeSpan.FromHours(4);
            FirstQuarterStart = firstQuarerStart;
        }
    }
}

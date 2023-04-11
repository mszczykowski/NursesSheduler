namespace NursesScheduler.Domain.DomainModels
{
    public sealed class DepartamentSettings
    {
        public int DepartamentSettingsId { get; set; }

        public TimeSpan WorkingTime { get; set; }

        public TimeSpan MaximalWeekWorkingTime { get; set; }

        public TimeSpan MinmalShiftBreak { get; set; }

        public int FirstQuarterStart { get; set; }

        public TimeOnly FirstShiftStartTime { get; set; }

        public int TargetNumberOfNursesOnShift { get; set; }

        public TimeSpan TargetMinimalMorningShiftLenght { get; set; }

        public int DefaultGeneratorRetryValue { get; set; }

        public int DepartamentId { get; set; }
        public Departament Departament { get; set; }

        public DepartamentSettings()
        {
            WorkingTime = new TimeSpan(7, 35, 0);
            MaximalWeekWorkingTime = new TimeSpan(24, 0, 0);
            MinmalShiftBreak = new TimeSpan(12, 0, 0);
            FirstQuarterStart = 2;
            FirstShiftStartTime = new TimeOnly(7, 0);
            TargetNumberOfNursesOnShift = 4;
            TargetMinimalMorningShiftLenght = new TimeSpan(6, 0, 0);
            DefaultGeneratorRetryValue = 4;
        }
    }
}

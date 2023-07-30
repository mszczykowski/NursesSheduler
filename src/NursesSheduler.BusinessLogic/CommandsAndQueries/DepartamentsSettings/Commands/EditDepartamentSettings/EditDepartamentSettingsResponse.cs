namespace NursesScheduler.BusinessLogic.CommandsAndQueries.DepartamentsSettings.Commands.EditDepartamentSettings
{
    public sealed class EditDepartamentSettingsResponse
    {
        public int SettingsId { get; set; }
        public TimeSpan WorkDayLength { get; set; }
        public TimeSpan MaximalWeekWorkTimeLength { get; set; }
        public TimeSpan MinmalShiftBreak { get; set; }
        public int FirstQuarterStart { get; set; }
        public TimeOnly FirstShiftStartTime { get; set; }
        public int TargetMinNumberOfNursesOnShift { get; set; }
        public TimeSpan TargetMinimalMorningShiftLenght { get; set; }
        public int DefaultGeneratorRetryValue { get; set; }
        public int DefaultGeneratorTimeOut { get; set; }
        public bool UseTeams { get; set; }
    }
}

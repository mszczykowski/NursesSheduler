namespace NursesScheduler.BusinessLogic.CommandsAndQueries.DepartamentsSettings.Queries.GetDepartamentSettings
{
    public sealed class GetDepartamentSettingsResponse
    {
        public int DepartamentSettingsId { get; set; }
        public TimeSpan WorkDayLength { get; set; }
        public TimeSpan MaximalWeekWorkTimeLength { get; set; }
        public TimeSpan MinmalShiftBreak { get; set; }
        public int FirstQuarterStart { get; set; }
        public TimeOnly FirstShiftStartTime { get; set; }
        public int TargetMinNumberOfNursesOnShift { get; set; }
        public TimeSpan TargetMinimalMorningShiftLenght { get; set; }
        public int DefaultGeneratorRetryValue { get; set; }
        public bool UseTeams { get; set; }
        public int DepartamentId { get; set; }
    }
}

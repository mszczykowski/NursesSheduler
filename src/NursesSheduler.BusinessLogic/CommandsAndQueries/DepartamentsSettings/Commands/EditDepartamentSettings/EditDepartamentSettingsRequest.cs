using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.DepartamentsSettings.Commands.EditDepartamentSettings
{
    public sealed class EditDepartamentSettingsRequest : IRequest<EditDepartamentSettingsResponse>
    {
        public int DepartamentSettingsId { get; set; }
        public TimeSpan WorkDayLength { get; set; }
        public TimeSpan MaximumWeekWorkTimeLength { get; set; }
        public TimeSpan MinimalShiftBreak { get; set; }
        public TimeSpan TargetMinimalMorningShiftLenght { get; set; }
        public TimeSpan DayShiftHolidayEligibleHours { get; set; }
        public TimeSpan NightShiftHolidayEligibleHours { get; set; }
        public bool UseTeams { get; set; }
        public int TargetMinNumberOfNursesOnShift { get; set; }
        public int DefaultGeneratorRetryValue { get; set; }
    }
}

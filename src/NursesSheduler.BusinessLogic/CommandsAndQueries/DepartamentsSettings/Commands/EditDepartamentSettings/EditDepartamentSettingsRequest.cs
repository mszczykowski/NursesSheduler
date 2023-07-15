using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.DepartamentsSettings.Commands.EditDepartamentSettings
{
    public sealed class EditDepartamentSettingsRequest : IRequest<EditDepartamentSettingsResponse>
    {
        public int DepartamentSettingsId { get; set; }
        public TimeSpan WorkDayLength { get; set; }
        public TimeSpan MaximalWeekWorkDayLength { get; set; }
        public TimeSpan MinmalShiftBreak { get; set; }
        public int TargetNumberOfNursesOnShift { get; set; }
        public TimeSpan TargetMinimalMorningShiftLenght { get; set; }
        public int DefaultGeneratorRetryValue { get; set; }
    }
}

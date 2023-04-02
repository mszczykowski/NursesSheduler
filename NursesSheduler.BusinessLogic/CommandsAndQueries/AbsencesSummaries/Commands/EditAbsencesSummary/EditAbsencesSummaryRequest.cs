using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Commands.EditAbsencesSummary
{
    public sealed class EditAbsencesSummaryRequest : IRequest<EditAbsencesSummaryResponse>
    {
        public int AbsencesSummaryId { get; set; }
        public int Year { get; set; }
        public int PTODays { get; set; }
        public TimeSpan PTOTimeUsed { get; set; }
        public TimeSpan PTOTimeLeftFromPreviousYear { get; set; }
    }
}

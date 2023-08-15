using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Commands.EditAbsencesSummary
{
    public sealed class EditAbsencesSummaryRequest : IRequest<EditAbsencesSummaryResponse>
    {
        public int AbsencesSummaryId { get; set; }
        public int Year { get; set; }
        public TimeSpan PTOTimeLeft { get; set; }
        public TimeSpan PTOTimeLeftFromPreviousYear { get; set; }
    }
}

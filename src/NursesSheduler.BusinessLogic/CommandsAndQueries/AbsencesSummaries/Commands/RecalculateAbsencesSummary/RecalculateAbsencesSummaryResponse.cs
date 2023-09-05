namespace NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Commands.RecalculateAbsencesSummary
{
    public sealed class RecalculateAbsencesSummaryResponse
    {
        public int AbsencesSummaryId { get; set; }
        public int Year { get; set; }
        public TimeSpan PTOTimeLeft { get; set; }
        public TimeSpan PTOTimeLeftFromPreviousYear { get; set; }
    }
}

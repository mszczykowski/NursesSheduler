namespace NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Commands.RecalculateAbsencesSummary
{
    public sealed class RecalculateAbsencesSummaryResponse
    {
        public int AbsencesSummaryId { get; set; }
        public int Year { get; set; }
        public int PTODays { get; set; }
        public TimeSpan PTOTimeUsed { get; set; }
        public TimeSpan PTOTimeLeftFromPreviousYear { get; set; }
    }
}

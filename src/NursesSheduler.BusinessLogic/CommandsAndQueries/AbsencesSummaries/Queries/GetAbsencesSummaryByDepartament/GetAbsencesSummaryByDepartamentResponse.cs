namespace NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Queries.GetAbsencesSummaryByDepartament
{
    public sealed class GetAbsencesSummaryByDepartamentResponse
    {
        public int NurseId { get; set; }
        public int Year { get; set; }
        public TimeSpan PTOTimeLeft { get; set; }
        public TimeSpan PTOTimeUsed { get; set; }
        public TimeSpan PTOTimeLeftFromPreviousYear { get; set; }
    }
}

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.AbsencesSummaries.Queries.GetAbsencesSummaryByDepartament
{
    public sealed class GetAbsencesSummaryByDepartamentResponse
    {
        public int NurseId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public ICollection<AbsencesSummaryResponse> YearlyAbsencesSummaries { get; set; }

        public sealed class AbsencesSummaryResponse
        {
            public int Year { get; set; }
            public TimeSpan PTOTime { get; set; }
            public TimeSpan PTOTimeUsed { get; set; }
            public TimeSpan PTOTimeLeftFromPreviousYear { get; set; }
        }
    }
}

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Quarters.Queries.GetQuarter
{
    public sealed class GetQuarterResponse
    {
        public int QuarterId { get; set; }
        public int QuarterNumber { get; set; }
        public int Year { get; set; }
        public int DepartamentId { get; set; }
    }
}

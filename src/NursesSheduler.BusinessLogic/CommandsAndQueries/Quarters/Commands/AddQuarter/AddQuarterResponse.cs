namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Quarters.Commands.AddQuarter
{
    public sealed class AddQuarterResponse
    {
        public int QuarterId { get; set; }
        public int QuarterNumber { get; set; }
        public int Year { get; set; }
        public int DepartamentId { get; set; }
    }
}

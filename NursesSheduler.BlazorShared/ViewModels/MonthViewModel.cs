namespace NursesScheduler.BlazorShared.ViewModels
{
    public sealed class MonthViewModel
    {
        public int MonthNumber { get; set; }
        public DayViewModel[] Days { get; set; }
        public int QuarterNubmber { get; set; }
        public int Year { get; set; }
        public string GetDate(int day)
        {
            return MonthNumber + "." + Days[day].DayOfMonth.ToString().PadLeft(2);
        }
    }
}

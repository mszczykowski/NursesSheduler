using NursesScheduler.BlazorShared.Abstracions;

namespace NursesScheduler.BlazorShared.ViewModels
{
    public sealed class DateViewModel : IMonthViewModel, IYearViewModel
    {
        public int MonthNumber { get; set; }
        public int YearNumber { get; set; }
    }
}

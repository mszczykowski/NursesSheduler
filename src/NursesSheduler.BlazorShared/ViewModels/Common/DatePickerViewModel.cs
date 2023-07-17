using NursesScheduler.BlazorShared.Abstracions;

namespace NursesScheduler.BlazorShared.ViewModels.Common
{
    public sealed class DatePickerViewModel : IMonthPickerViewModel, IYearPickerViewModel
    {
        public int MonthNumber { get; set; }
        public int YearNumber { get; set; }
    }
}

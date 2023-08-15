using NursesScheduler.BlazorShared.Abstracions;

namespace NursesScheduler.BlazorShared.Models.ViewModels.Common
{
    internal class YearPickerViewModel : IYearPickerViewModel
    {
        public int YearNumber { get; set; }

        public YearPickerViewModel()
        {
            YearNumber = DateTime.Now.Year;
        }

        public YearPickerViewModel(int year)
        {
            YearNumber = year;
        }
    }
}

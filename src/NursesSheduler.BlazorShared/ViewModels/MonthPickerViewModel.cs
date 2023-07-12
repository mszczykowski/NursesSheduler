using NursesScheduler.BlazorShared.Abstracions;

namespace NursesScheduler.BlazorShared.ViewModels
{
    public sealed class MonthPickerViewModel : IMonthPickerViewModel
    {
        public int MonthNumber { get; set; }

        public MonthPickerViewModel()
        {
            MonthNumber = 1;
        }

        public MonthPickerViewModel(int monthNumber)
        {
            if(monthNumber >= 1 && monthNumber <= 12)
            {
                MonthNumber = monthNumber;
            }
            else
            {
                MonthNumber = 1;
            }
        }
    }
}

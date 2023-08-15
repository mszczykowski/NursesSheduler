using NursesScheduler.BlazorShared.Abstracions;
using NursesScheduler.BlazorShared.Models.Enums;

namespace NursesScheduler.BlazorShared.Models.ViewModels.Common
{
    public sealed class DatePickerViewModel : IMonthPickerViewModel, IYearPickerViewModel
    {
        public int MonthNumber { get; set; }
        public int YearNumber { get; set; }

        public int PreviousMonth => MonthNumber - 1 > 0 ? MonthNumber - 1 : 12;
        public int PreviousYear => MonthNumber - 1 > 0 ? YearNumber : YearNumber - 1;

        public override string ToString()
        {
            return $"{((Months)MonthNumber).ToString()} {YearNumber}";
        }
    }
}

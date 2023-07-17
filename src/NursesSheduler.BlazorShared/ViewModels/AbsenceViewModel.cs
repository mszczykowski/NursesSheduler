using NursesScheduler.BlazorShared.ViewModels.Enums;
using System.Text;

namespace NursesScheduler.BlazorShared.ViewModels
{
    public sealed class AbsenceViewModel
    {
        public int AbsenceId { get; set; }
        public int Month { get; set; }
        public IEnumerable<int> Days { get; set; }
        public TimeSpan WorkingHoursToAssign { get; set; }
        public TimeSpan AssignedWorkingHours { get; set; }
        public AbsenceTypes Type { get; set; }
        public bool IsClosed { get; set; }
        public int AbsencesSummaryId { get; set; }

        public int Lenght => Days.Count();

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            foreach(var day in Days.OrderBy(d => d))
            {
                stringBuilder.Append(day);
                stringBuilder.Append(',');
            }
            if (stringBuilder.Length > 0)
            {
                stringBuilder.Remove(stringBuilder.Length - 1, 1);
            }
            return stringBuilder.ToString();
        }
    }
}

﻿namespace NursesScheduler.BlazorShared.ViewModels
{
    public sealed class AbsenceViewModel
    {
        public int AbsenceId { get; set; }
        public int Month { get; set; }
        public ICollection<int> Days { get; set; }
        public TimeSpan WorkingHoursToAssign { get; set; }
        public TimeSpan AssignedWorkingHours { get; set; }
        public int AbsencesSummaryId { get; set; }

        public int Lenght => Days.Count;

        public override string ToString()
        {
            return $"{Days.Min()}.{Month} - {Days.Max()}.{Month}";
        }
    }
}

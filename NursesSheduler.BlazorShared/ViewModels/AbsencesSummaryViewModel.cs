﻿namespace NursesScheduler.BlazorShared.ViewModels
{
    public sealed class AbsencesSummaryViewModel
    {
        public int AbsencesSummaryId { get; set; }
        public int Year { get; set; }
        public int PTODays => PTOTime.Days;
        public TimeSpan PTOTime { get; set; }
        public TimeSpan PTOTimeUsed { get; set; }
        public TimeSpan PTOTimeLeftFromPreviousYear { get; set; }
        public TimeSpan PTOLeft => PTOTime + PTOTimeLeftFromPreviousYear - PTOTimeUsed;
        public List<AbsenceViewModel> Absences { get; set; }
    }
}

namespace NursesScheduler.BlazorShared.ViewModels
{
    public sealed class YearlyAbsencesSummaryViewModel
    {
        public int YearlyAbsencesId { get; set; }
        public int Year { get; set; }
        public int PTODays { get; set; }
        public TimeSpan PTO { get; set; }
        public TimeSpan PTOUsed { get; set; }
        public TimeSpan PTOLeftFromPreviousYear { get; set; }
        public TimeSpan PTOLeft => PTO + PTOLeftFromPreviousYear - PTOUsed;
        public ICollection<AbsenceViewModel> Absences { get; set; }
    }
}

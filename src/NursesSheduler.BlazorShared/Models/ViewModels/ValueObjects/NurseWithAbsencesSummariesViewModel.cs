namespace NursesScheduler.BlazorShared.Models.ViewModels
{
    public sealed class NurseWithAbsencesSummariesViewModel
    {
        public int NurseId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public ICollection<AbsencesSummaryViewModel> AbsencesSummaries { get; set; }
    }
}

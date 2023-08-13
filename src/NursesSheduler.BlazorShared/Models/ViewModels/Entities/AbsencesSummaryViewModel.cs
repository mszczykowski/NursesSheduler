using System.ComponentModel.DataAnnotations;

namespace NursesScheduler.BlazorShared.Models.ViewModels.Entities
{
    public sealed class AbsencesSummaryViewModel
    {
        public int NurseId { get; set; }
        public int AbsencesSummaryId { get; set; }
        public int Year { get; set; }

        [Required(ErrorMessage = "Należy wpisać ilość wykorzystanego urlopu")]
        [Range(typeof(TimeSpan), "00:00:00", "10675199.02:48:05.4775807", ErrorMessage = "Wartość musi być większa od 0")]
        public TimeSpan PTOTimeLeft { get; set; }

        [Required(ErrorMessage = "Należy wpisać ilość zaległego")]
        [Range(typeof(TimeSpan), "00:00:00", "10675199.02:48:05.4775807", ErrorMessage = "Wartość musi być większa od 0")]
        public TimeSpan PTOTimeLeftFromPreviousYear { get; set; }

        public ICollection<AbsenceViewModel> Absences { get; set; }

        public AbsencesSummaryViewModel()
        {

        }

        public AbsencesSummaryViewModel(AbsencesSummaryViewModel summary)
        {
            NurseId = summary.NurseId;
            AbsencesSummaryId = summary.AbsencesSummaryId;
            Year = summary.Year;
            PTOTimeLeft = summary.PTOTimeLeft;
            PTOTimeLeftFromPreviousYear = summary.PTOTimeLeftFromPreviousYear;
        }
    }
}

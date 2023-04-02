using System.ComponentModel.DataAnnotations;

namespace NursesScheduler.BlazorShared.ViewModels
{
    public sealed class AbsencesSummaryEditViewModel
    {
        public int AbsencesSummaryId { get; set; }
        public int Year { get; set; }
        [Required(ErrorMessage = "Należy wpisać ilość dni przysługującego urlopu")]
        [Range(1, 30, ErrorMessage = "Przysługujący urlop musi być dłuszy niż 1 dzień i krótszy niż 30 dni")]
        public int PTODays { get; set; }
        [Required(ErrorMessage = "Należy wpisać ilość wykorzystanego urlopu")]
        [Range(typeof(TimeSpan), "00:00:00", "10675199.02:48:05.4775807", ErrorMessage = "Wartość musi być większa od 0")]
        public TimeSpan PTOTimeUsed { get; set; }
        [Required(ErrorMessage = "Należy wpisać ilość zaległego")]
        [Range(typeof(TimeSpan), "00:00:00", "10675199.02:48:05.4775807", ErrorMessage = "Wartość musi być większa od 0")]
        public TimeSpan PTOTimeLeftFromPreviousYear { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace NursesScheduler.BlazorShared.Models.Enums
{
    public enum SolverEvents
    {
        [Display(Name = "Ropoczęto generowanie")]
        Started,
        [Display(Name = "Przekroczono maksymalny czas generowania")]
        TimedOut,
        [Display(Name = "Użytkownik przerwał generowanie")]
        CanceledByUser,
        [Display(Name = "Wykożystano wszystkie próby")]
        RetriesExhausted,
        [Display(Name = "Nieznaleziono rozwiązania")]
        SolutionNotFound,
        [Display(Name = "Znaleziono rozwiązanie")]
        SolutionFound,
        [Display(Name = "Zakończono generowanie")]
        Finished,
        [Display(Name = "Anulowano generowanie, powód:")]
        Aborted,
    }
}

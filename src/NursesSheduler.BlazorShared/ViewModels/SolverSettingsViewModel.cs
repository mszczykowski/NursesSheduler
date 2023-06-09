using System.ComponentModel.DataAnnotations;

namespace NursesScheduler.BlazorShared.ViewModels
{
    public sealed class SolverSettingsViewModel
    {
        [Required(ErrorMessage = "Należy wpisać wartość")]
        [Range(1, 30, ErrorMessage = "Minimalna liczba powtórzeń to 1, maksymalna 30")]
        public int NumberOfRetries { get; set; }
        public string GeneratorSeed { get; set; }
    }
}

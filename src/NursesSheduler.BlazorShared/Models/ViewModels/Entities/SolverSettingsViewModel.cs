using System.ComponentModel.DataAnnotations;

namespace NursesScheduler.BlazorShared.Models.ViewModels.Entities
{
    public sealed class SolverSettingsViewModel
    {
        [Required(ErrorMessage = "Należy wpisać wartość")]
        [Range(1, 30, ErrorMessage = "Minimalna liczba powtórzeń to 1, maksymalna 30")]
        public int NumberOfRetries { get; set; }
        public string GeneratorSeed { get; set; }
        public bool UseOwnSeed { get; set; }

        public SolverSettingsViewModel(int numberOfRetries)
        {
            NumberOfRetries = numberOfRetries;
        }

        public SolverSettingsViewModel(SolverSettingsViewModel solverSettingsViewModel)
        {
            NumberOfRetries = solverSettingsViewModel.NumberOfRetries;
            GeneratorSeed = solverSettingsViewModel.GeneratorSeed;
            UseOwnSeed = solverSettingsViewModel.UseOwnSeed;
        }
    }
}

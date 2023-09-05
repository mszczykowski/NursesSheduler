using System.ComponentModel.DataAnnotations;

namespace NursesScheduler.BlazorShared.Models.ViewModels.Entities
{
    public sealed class SolverSettingsViewModel
    {
        [Required(ErrorMessage = "Należy wpisać wartość")]
        [Range(1, 30, ErrorMessage = "Minimalna liczba powtórzeń to 1, maksymalna 30")]
        public int NumberOfRetries { get; set; }
        public string GeneratorSeed { get; set; }

        private bool _useOwnSeed;
        public bool UseOwnSeed
        {
            get => _useOwnSeed;
            set
            {
                _useOwnSeed = value;
                if (_useOwnSeed)
                {
                    _previousNumberOfRetries = NumberOfRetries;
                    NumberOfRetries = 1;
                }
                else if(_previousNumberOfRetries > 0)
                {
                    GeneratorSeed = "";
                    NumberOfRetries = _previousNumberOfRetries;
                }
            }
        }

        private int _previousNumberOfRetries;

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

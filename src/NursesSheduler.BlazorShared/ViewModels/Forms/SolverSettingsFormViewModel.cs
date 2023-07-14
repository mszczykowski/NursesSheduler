using System.ComponentModel.DataAnnotations;

namespace NursesScheduler.BlazorShared.ViewModels.Forms
{
    internal sealed class SolverSettingsFormViewModel
    {
        [ValidateComplexType]
        public SolverSettingsViewModel SolverSettings { get; set; }

        public SolverSettingsFormViewModel(SolverSettingsViewModel solverSettings)
        {
            SolverSettings = new SolverSettingsViewModel(solverSettings);
        }
    }
}

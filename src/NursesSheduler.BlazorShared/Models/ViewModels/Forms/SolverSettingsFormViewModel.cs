using NursesScheduler.BlazorShared.Models.ViewModels.Entities;
using System.ComponentModel.DataAnnotations;

namespace NursesScheduler.BlazorShared.Models.ViewModels.Forms
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

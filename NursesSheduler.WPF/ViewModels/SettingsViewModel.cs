using NursesScheduler.WPF.Commands;
using NursesScheduler.WPF.Commands.Common;
using NursesScheduler.WPF.Services.Implementation;
using NursesScheduler.WPF.Services.Interfaces;
using System.Windows.Input;

namespace NursesScheduler.WPF.ViewModels
{
    internal sealed class SettingsViewModel : ViewModelBase
    {
        public ICommand RemoveDbCommand { get; }
        public ICommand BackCommand { get; }

        public SettingsViewModel(NavigationService<LogInViewModel> logInNavigationService, 
            NavigationService<RegisterViewModel> registerNavigationService, IDatabaseService databaseService, 
            IPasswordService passwordService)
        {
            RemoveDbCommand = new RemoveDbCommand(databaseService, registerNavigationService, passwordService);

            BackCommand = new NavigateCommand<LogInViewModel>(logInNavigationService);
        }
    }
}

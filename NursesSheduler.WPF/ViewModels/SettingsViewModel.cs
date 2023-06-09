using NursesScheduler.WPF.Commands;
using NursesScheduler.WPF.Commands.Common;
using NursesScheduler.WPF.Constants;
using NursesScheduler.WPF.Services.Implementation;
using NursesScheduler.WPF.Services.Interfaces;
using System.Windows.Input;

namespace NursesScheduler.WPF.ViewModels
{
    internal sealed class SettingsViewModel : ViewModelBase
    {
        public ICommand RemoveDbCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand SetPolishLanguageCommand { get; }
        public ICommand SetEnglishLanguageCommand { get; }


        public SettingsViewModel(NavigationService<LogInViewModel> logInNavigationService, 
            NavigationService<RegisterViewModel> registerNavigationService,
            NavigationService<LogInViewModel> navigationService,
            IDatabaseService databaseService, 
            IPasswordService passwordService)
        {
            RemoveDbCommand = new RemoveDbCommand(databaseService, registerNavigationService, passwordService);
            SetEnglishLanguageCommand = new ChangeLanguageCommand(Languages.US, navigationService);
            SetPolishLanguageCommand = new ChangeLanguageCommand(Languages.PL, navigationService);

            BackCommand = new NavigateCommand<LogInViewModel>(logInNavigationService);
        }
    }
}

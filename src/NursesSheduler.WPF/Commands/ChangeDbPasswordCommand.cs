using NursesScheduler.WPF.Commands.CommandsBase;
using NursesScheduler.WPF.Services.Implementation;
using NursesScheduler.WPF.Services.Interfaces;
using NursesScheduler.WPF.ViewModels;
using System.Windows;

namespace NursesScheduler.WPF.Commands
{
    internal sealed class ChangeDbPasswordCommand : CommandBase
    {
        private readonly IDatabaseService _databaseService;
        private readonly IPasswordService _passwordService;
        private readonly NavigationService<LogInViewModel> _logInViewNavigationService;
        private readonly ChangePasswordViewModel _changePasswordViewModel;

        public ChangeDbPasswordCommand(ChangePasswordViewModel changePasswordViewModel, IDatabaseService databaseService, 
            IPasswordService passwordService, NavigationService<LogInViewModel> logInViewNavigationService)
        {
            _databaseService = databaseService;
            _passwordService = passwordService;
            _logInViewNavigationService = logInViewNavigationService;
            _changePasswordViewModel = changePasswordViewModel;
        }

        public override async void Execute(object? parameter)
        {
            _changePasswordViewModel.ValidateForm();

            if (_changePasswordViewModel.HasErrors) 
                return;

            await _databaseService.ChangeDbPassword(_changePasswordViewModel.OldPassword,
                _changePasswordViewModel.NewPassword);

            _passwordService.TryRemovePassword();

            _logInViewNavigationService.Navigate();

            MessageBox.Show((string)Application.Current.FindResource("passwd_changed"));
        }
    }
}

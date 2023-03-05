using NursesScheduler.WPF.Commands.CommandsBase;
using NursesScheduler.WPF.Services.Interfaces;
using NursesScheduler.WPF.ViewModels;
using System.Windows;

namespace NursesScheduler.WPF.Commands
{
    internal sealed class LogInCommand : CommandBase
    {
        private readonly LogInViewModel _logInViewModel;
        private readonly IDatabaseService _databaseService;
        private readonly IPasswordService _passwordService;

        public LogInCommand(LogInViewModel logInViewModel, IDatabaseService databaseService, 
            IPasswordService passwordService)
        {
            _logInViewModel = logInViewModel;
            _databaseService = databaseService;
            _passwordService = passwordService;
        }

        public override async void Execute(object? parameter)
        {
            _logInViewModel.ValidateForm();

            if (_logInViewModel.HasErrors) return;

            var connectionString = await _databaseService.TryGetConnectionStingFromPassword(_logInViewModel.Password);

            if (connectionString == null)
            {
                MessageBox.Show((string)Application.Current.FindResource("wrong_passwd"));
                return;
            }

            if(_logInViewModel.SavePassword)
            {
                _passwordService.SavePassword(_logInViewModel.Password);
            }
            else
            {
                _passwordService.TryRemovePassword();
            }

            var mainWindow = new MainWindow();
            mainWindow.DataContext = new BlazorAppViewModel(connectionString);
            mainWindow.Show();

            ((App)Application.Current).DisposeHost();
        }
    }
}

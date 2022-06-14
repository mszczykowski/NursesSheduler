using NursesScheduler.WPF.Services.Interfaces;
using NursesScheduler.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NursesScheduler.WPF.Commands
{
    internal class LogInCommand : CommandBase
    {
        private readonly LogInViewModel _logInViewModel;
        private readonly IPasswordService _passwordService;
        private readonly IDatabaseService _databaseService;

        public LogInCommand(LogInViewModel logInViewModel, IPasswordService passwordService, IDatabaseService databaseService)
        {
            _logInViewModel = logInViewModel;
            _passwordService = passwordService;
            _databaseService = databaseService;
        }

        public override async void Execute(object? parameter)
        {
            _logInViewModel.ValidateForm();

            if (_logInViewModel.HasErrors) return;

            var connectionString = _databaseService.GetConnectionString(_logInViewModel.Password);

            if (!(await _passwordService.IsPasswordValid(connectionString)))
            {
                MessageBox.Show("Password invalid");
                return;
            }

            var mainWindow = new MainWindow();
            mainWindow.DataContext = new BlazorAppViewModel(connectionString);
            mainWindow.Show();

            ((App)Application.Current).DisposeHost();
        }
    }
}

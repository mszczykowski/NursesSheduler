using NursesScheduler.WPF.Services.Interfaces;
using NursesScheduler.WPF.ViewModels;
using System.Windows;

namespace NursesScheduler.WPF.Commands
{
    internal class CreateDbCommand : CommandBase
    {
        private readonly RegisterViewModel _registerViewModel;
        private readonly IDatabaseService _databaseManager;
        private readonly INavigationService _navigationService;

        public CreateDbCommand(RegisterViewModel registerViewModel, IDatabaseService databaseManager, INavigationService navigationService)
        {
            _registerViewModel = registerViewModel;
            _databaseManager = databaseManager;
            _navigationService = navigationService;
        }

        public override async void Execute(object? parameter)
        {
            _registerViewModel.ValidateForm();

            if (_registerViewModel.HasErrors) return;

            await _databaseManager.CreateDb(_registerViewModel.Password);

            _navigationService.Navigate();

            MessageBox.Show("Password created. You can log in now.");
        }
    }
}

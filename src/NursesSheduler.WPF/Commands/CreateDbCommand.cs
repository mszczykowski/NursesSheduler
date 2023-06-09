using NursesScheduler.WPF.Commands.CommandsBase;
using NursesScheduler.WPF.Services.Interfaces;
using NursesScheduler.WPF.ViewModels;
using System.Threading.Tasks;
using System.Windows;

namespace NursesScheduler.WPF.Commands
{
    internal sealed class CreateDbCommand : AsyncCommandBase
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

        public override async Task ExecuteAsync(object? parameter)
        {
            _registerViewModel.ValidateForm();

            if (_registerViewModel.HasErrors) return;

            _registerViewModel.IsLoading = true;

            await Task.Run(async () => await _databaseManager.CreateDb(_registerViewModel.Password));

            _navigationService.Navigate();

            MessageBox.Show((string)Application.Current.FindResource("passwd_created"));
        }
    }
}

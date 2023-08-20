using NursesScheduler.WPF.Commands.CommandsBase;
using NursesScheduler.WPF.Services.Interfaces;
using System.Windows;

namespace NursesScheduler.WPF.Commands
{
    internal sealed class RemoveDbCommand : CommandBase
    {
        private readonly IDatabaseService _databaseService;
        private readonly IPasswordService _passwordService;
        private readonly INavigationService _navigationService;

        public RemoveDbCommand(IDatabaseService databaseService, INavigationService navigationService, 
            IPasswordService passwordService)
        {
            _databaseService = databaseService;
            _navigationService = navigationService;
            _passwordService = passwordService;
        }

        public override void Execute(object? parameter)
        {
            if (MessageBox.Show((string)Application.Current.FindResource("delete_db_message"), 
                (string)Application.Current.FindResource("clear"), MessageBoxButton.YesNo,
                MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }
            
            try
            {
                _databaseService.DeleteDb();
            }
            catch
            {
                MessageBox.Show((string)Application.Current.FindResource("delete_db_error"), 
                    (string)Application.Current.FindResource("clear"));
                return;
            }
            
            _passwordService.TryRemovePassword();

            _navigationService.Navigate();
        }
    }
}

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
            if (MessageBox.Show("Usunąć wszytskie dane? Tej operacji nie da się cofnąć", "Wyczyść", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;

            _databaseService.DeleteDb();
            _passwordService.TryRemovePassword();

            _navigationService.Navigate();
        }
    }
}

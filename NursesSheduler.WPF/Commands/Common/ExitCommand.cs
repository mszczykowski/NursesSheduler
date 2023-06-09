using NursesScheduler.WPF.Commands.CommandsBase;
using System.Windows;

namespace NursesScheduler.WPF.Commands.Common
{
    internal sealed class ExitCommand : CommandBase
    {
        public override void Execute(object? parameter)
        {
            if (MessageBox.Show((string)Application.Current.FindResource("close"), 
                (string)Application.Current.FindResource("exit"), MessageBoxButton.YesNo, 
                MessageBoxImage.Question) == MessageBoxResult.No) return;

            System.Windows.Application.Current.Shutdown();
        }
    }
}

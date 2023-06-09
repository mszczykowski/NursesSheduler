using NursesScheduler.WPF.Commands.CommandsBase;
using NursesScheduler.WPF.Services.Interfaces;
using System.Windows;

namespace NursesScheduler.WPF.Commands
{
    internal sealed class ChangeLanguageCommand : CommandBase
    {
        private readonly string _language;
        private readonly INavigationService _navigationService;
        public ChangeLanguageCommand(string language, INavigationService navigationService)
        {
            _language = language;
            _navigationService = navigationService;
        }

        public override void Execute(object? parameter)
        {
            ((App)Application.Current).SetLanguageDictionary(_language);
            _navigationService.Navigate();
        }
    }
}

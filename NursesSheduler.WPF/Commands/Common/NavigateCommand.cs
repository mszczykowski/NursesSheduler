using NursesScheduler.WPF.Services.Implementations;
using NursesScheduler.WPF.ViewModels;

namespace NursesScheduler.WPF.Commands.Common
{
    internal class NavigateCommand<TViewModel> : CommandBase where TViewModel : ViewModelBase
    {
        private readonly NavigationService<TViewModel> _navigationService;

        public NavigateCommand(NavigationService<TViewModel> navigationService)
        {
            _navigationService = navigationService;
        }
        public override void Execute(object? parameter)
        {
            _navigationService.Navigate();
        }
    }
}

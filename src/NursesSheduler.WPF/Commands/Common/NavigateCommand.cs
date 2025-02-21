﻿using NursesScheduler.WPF.Commands.CommandsBase;
using NursesScheduler.WPF.Services.Implementation;
using NursesScheduler.WPF.ViewModels;

namespace NursesScheduler.WPF.Commands.Common
{
    internal sealed class NavigateCommand<TViewModel> : CommandBase where TViewModel : ViewModelBase
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

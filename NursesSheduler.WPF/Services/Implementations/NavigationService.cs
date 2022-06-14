﻿using NursesScheduler.WPF.Services.Interfaces;
using NursesScheduler.WPF.Stores;
using NursesScheduler.WPF.ViewModels;
using System;

namespace NursesScheduler.WPF.Services.Implementations
{
    internal class NavigationService<TViewModel> : INavigationService where TViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly Func<TViewModel> _createViewModel;

        public NavigationService(NavigationStore navigationStore, Func<TViewModel> createViewModel)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
        }
        public void Navigate()
        {
            _navigationStore.CurrentViewModel = _createViewModel();
        }
    }
}

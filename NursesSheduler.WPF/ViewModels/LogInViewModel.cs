using NursesScheduler.WPF.Commands;
using NursesScheduler.WPF.Commands.Common;
using NursesScheduler.WPF.Services.Implementations;
using NursesScheduler.WPF.Services.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NursesScheduler.WPF.ViewModels
{
    internal class LogInViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private string _password;
        public string Password 
        { 
            get => _password; 
            set
            {
                _password = value;
                ValidatePassword();
                OnPropertyChanged(nameof(Password));
            }
        }

        private bool _savePassword;
        public bool SavePassword 
        { 
            get => _savePassword; 
            set
            {
                _savePassword = value;
                OnPropertyChanged(nameof(SavePassword));
            }
        }

        public ICommand NavigateToSettingsCommand { get; set; }
        public ICommand NavigateToChangePasswordCommand { get; set; }
        public ICommand LogInCommand { get; set; }
        public ICommand ExitCommand { get; set; }

        public LogInViewModel(NavigationService<SettingsViewModel> settingsViewNavigationService,
            NavigationService<ChangePasswordViewModel> changePasswordViewNavigationService,
            IDatabaseService databaseManager, IPasswordService passwordManager)
        {
            NavigateToSettingsCommand = new NavigateCommand<SettingsViewModel>(settingsViewNavigationService);

            NavigateToChangePasswordCommand = new NavigateCommand<ChangePasswordViewModel>(changePasswordViewNavigationService);

            LogInCommand = new LogInCommand(this, passwordManager, databaseManager);

            ExitCommand = new ExitCommand();

            _errorsViewModel = new ErrorsViewModel();

            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
        }

        private readonly ErrorsViewModel _errorsViewModel;

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public bool HasErrors => _errorsViewModel.HasErrors;

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsViewModel.GetErrors(propertyName);
        }

        private void ErrorsViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
        }

        public void ValidateForm()
        {
            ValidatePassword();
        }

        private void ValidatePassword()
        {
            _errorsViewModel.ClearErrors(nameof(Password));

            if (string.IsNullOrEmpty(Password)) _errorsViewModel.AddError(nameof(Password), "Password can't be empty");
        }
    }
}
